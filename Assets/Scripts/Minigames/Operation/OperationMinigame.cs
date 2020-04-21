using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CheeseTeam {
    public class OperationMinigame : Minigame {

        public AudioClip squish;
        private AudioSource audioSource;

        private bool isPlaying = true;

        public float minOrganScale = 0.5f;
        public float maxOrganScale = 1.5f;
        private float organScale = 1.0f;
        public float interOrganSpacing = 0.125f;

        public Texture2D mouseCursor;

        public GameObject organPrefab;
        public GameObject dragZonePrefab;
        private DraggableObject grabbedObject;
        private List<Organ> organs;
        private List<DragZone> dragZones;

        public Transform organSpawnCenter;
        public Transform dragZoneSpawnCenter;
        public Vector2 dragZoneSpawnRange;

        public Sprite[] organTextures;
        public Sprite[] dragZoneTextures;

        public int numOrgansToSpawn = 15;
        public int minNumOrgans = 3;
        public int maxNumOrgans = 15;
        public int maxDifficulty = 30;
        public int difficultiesPerStep = 2;


        public override bool Setup(int difficulty) {
            base.Setup(difficulty);
            audioSource = GetComponent<AudioSource>();
            organs = new List<Organ>();
            dragZones = new List<DragZone>();

            // Override cursors
            Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.ForceSoftware);

            // Handle difficulty
            var step = (maxOrganScale - minOrganScale) / (maxDifficulty / difficultiesPerStep);
            var adjustedDifficulty = difficulty / difficultiesPerStep;
            organScale = maxOrganScale - (step * adjustedDifficulty); // Scale down the organs as the game gets harder
            numOrgansToSpawn = Mathf.Clamp(difficulty / difficultiesPerStep, minNumOrgans, maxNumOrgans); // Add more organs every `difficultiesPerStep` difficulties
            Debug.Log($"Operation difficulty is {difficulty}");
            Debug.Log($"Adjusted difficulty is {adjustedDifficulty}");
            Debug.Log($"Spawning {numOrgansToSpawn} organs");
            Debug.Log($"Organ scale set to {organScale}");

            // Spawn organs and organ zones
            for (int i = 0; i < numOrgansToSpawn; i++) {
                var organIndex = UnityEngine.Random.Range(0, organTextures.Length - 1);
                var desiredTag = "Organ " + organIndex.ToString();

                // Create organ
                var yIndex = i % 5;
                var xIndex = i / 5;
                var organ = MakeOrgan(desiredTag, new Vector3(
                    organSpawnCenter.position.x + xIndex,
                    organSpawnCenter.position.y - (yIndex + (interOrganSpacing * yIndex)),
                    organSpawnCenter.position.z
                ));
                organ.gameObject.AttachSprite(organTextures[organIndex], 50);
                organ.transform.localScale = new Vector3(organScale, organScale, organScale);
                organs.Add(organ);

                // Create drag zone for organ
                var pos = MinigameCommon.RandomPointOnXYPlane(dragZoneSpawnCenter.position, dragZoneSpawnRange, 1f);
                // Keep assigning the position until we don't collide with any other drag zones
                int guard = 0;
                while (dragZones.Count > 0) {
                    var hasCollision = false;
                    foreach (var zone in dragZones) {
                        if (Vector3.Distance(zone.transform.position, pos) < (organScale / 2) * Mathf.Sqrt(2)) {
                            hasCollision = true;
                        }
                    }
                    if (hasCollision) {
                        pos = MinigameCommon.RandomPointOnXYPlane(dragZoneSpawnCenter.position, dragZoneSpawnRange, 1f);
                    } else {
                        break;
                    }
                    guard++;
                    if (guard > 1000) {
                        Debug.LogWarning("Guarded against infinite loop");
                        break;
                    }
                }
                var dragZone = MakeDragZone(desiredTag, pos);
                dragZone.gameObject.AttachSprite(dragZoneTextures[organIndex], 40);
                dragZone.transform.localScale = new Vector3(organScale, organScale, organScale);
                dragZones.Add(dragZone);
            }

            return true;
        }

        public override void TimerEnd()
        {
            OnGameLose?.Invoke();
            base.TimerEnd();
        }

        public override void StartGame() {
            base.StartGame();
        }

        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                grabbedObject = MinigameCommon.RaycastFromMouse(typeof(DraggableObject)) as DraggableObject;
                if (grabbedObject != null) {
                    grabbedObject.InHand = true;
                }
            }

            if (Input.GetMouseButtonUp(0) && grabbedObject != null) {
                grabbedObject.InHand = false;
                grabbedObject = null;
                PlaySquish();
            }

            if (Input.GetMouseButtonUp(0)) {
                // Check if all organs are in the right spots
                var correct = 0;
                foreach(DragZone zone in dragZones) {
                    if (zone.hasDesiredObject)
                        correct++;
                }
                if (correct == dragZones.Count && isPlaying) {
                    isPlaying = false;
                    Debug.Log("Game won!");
                    OnGameWin();
                }
            }
        }

        void OnDestroy() {
            Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);
        }

        Organ MakeOrgan(string name, Vector3 pos) {
            var obj = Instantiate(organPrefab, pos, Quaternion.identity);
            obj.name = name;
            obj.GetComponent<DragTag>().id = name;
            return obj.GetComponent<Organ>();
        }

        DragZone MakeDragZone(string name, Vector3 pos) {
            var obj = Instantiate(dragZonePrefab, pos, Quaternion.identity);
            obj.name = name;
            var zone = obj.GetComponent<DragZone>();
            zone.desiredObjectTag = name;
            return zone;
        }

        private void PlaySquish() {
            audioSource.volume = 0.5f;
            audioSource.PlayOneShot(squish);
        }

        private void OnDrawGizmos() {
            // Draw lines representing drag zone closeness
            if (dragZones != null) {
                Gizmos.color = Color.green;
                // Build unique zone<->zone pairings
                HashSet<KeyValuePair<DragZone, DragZone>> zoneSet = new HashSet<KeyValuePair<DragZone, DragZone>>();
                foreach (DragZone zone in dragZones)
                    foreach (DragZone innerZone in dragZones) 
                        zoneSet.Add(new KeyValuePair<DragZone, DragZone>(zone, innerZone));
                
                foreach (KeyValuePair<DragZone, DragZone> zonePair in zoneSet) {
                    Gizmos.color = Vector3.Distance(zonePair.Key.transform.position, zonePair.Value.transform.position) >= (organScale / 2) * Mathf.Sqrt(2) ? Color.green : Color.red;
                    Gizmos.DrawLine(zonePair.Key.transform.position, zonePair.Value.transform.position);
                }
            }

            // Draw drag zone spawn area
            if (dragZoneSpawnCenter != null || dragZoneSpawnRange != null) {
                Gizmos.color = Color.green;
                var zonePos = dragZoneSpawnCenter.position;
                MinigameCommon.DrawGizmoBox(
                    new Vector3(zonePos.x + -dragZoneSpawnRange.x, zonePos.y + -dragZoneSpawnRange.y, 0f),
                    new Vector3(zonePos.x +  dragZoneSpawnRange.x, zonePos.y + -dragZoneSpawnRange.y, 0f),
                    new Vector3(zonePos.x +  dragZoneSpawnRange.x, zonePos.y +  dragZoneSpawnRange.y, 0f),
                    new Vector3(zonePos.x + -dragZoneSpawnRange.x, zonePos.y +  dragZoneSpawnRange.y, 0f)
                );
            }

            // Draw organ spawn area
            if (organSpawnCenter != null) {
                Gizmos.color = Color.green;
                var organPos = organSpawnCenter.position;
                // 5 is max number of organs in a column
                // Going from center to center, so we lose 1 unit of height
                var yOffset = (5 - 1) + (interOrganSpacing * (5 - 1));
                var xOffset = ((numOrgansToSpawn - 1) / 5);
                MinigameCommon.DrawGizmoBox(
                    new Vector3(organPos.x - 0.5f,           organPos.y + 0.5f,           0f), // Top-left
                    new Vector3(organPos.x - 0.5f,           organPos.y - 0.5f - yOffset, 0f), // Bottom-left
                    new Vector3(organPos.x + 0.5f + xOffset, organPos.y - 0.5f - yOffset, 0f), // Bottom-right
                    new Vector3(organPos.x + 0.5f + xOffset, organPos.y + 0.5f,           0f) // Top-right
                );
                // Draw organs in spawn area
                for (int i = 0; i < numOrgansToSpawn; i++) {
                    var yIndex = i % 5;
                    var xIndex = i / 5;
                    var pos = new Vector3(
                        organSpawnCenter.position.x + xIndex,
                        organSpawnCenter.position.y - (yIndex + (interOrganSpacing * yIndex)),
                        organSpawnCenter.position.z
                    );
                    Gizmos.DrawWireSphere(pos, organScale / 2 / 2);
                }
            }

        }

    }
}
