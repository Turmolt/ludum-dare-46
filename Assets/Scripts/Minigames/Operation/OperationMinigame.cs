using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CheeseTeam {
    public class OperationMinigame : Minigame {

        private bool isPlaying = true;

        public int maxSpawnedOrgans = 5;
        public float organScale = 1.0f;
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


        public override bool Setup(int difficulty) {
            base.Setup(difficulty);
            organs = new List<Organ>();
            dragZones = new List<DragZone>();

            // Override cursors
            Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.ForceSoftware);

            // Spawn organs and organ zones
            for (int i = 0; i < maxSpawnedOrgans; i++) {
                var organIndex = UnityEngine.Random.Range(0, organTextures.Length - 1);
                var desiredTag = "Organ " + organIndex.ToString();

                // Create organ
                var organ = MakeOrgan(desiredTag, new Vector3(
                    organSpawnCenter.position.x,
                    organSpawnCenter.position.y - i - (interOrganSpacing * i),
                    organSpawnCenter.position.z
                ));
                organ.transform.localScale = new Vector3(organScale, organScale, organScale);
                organ.gameObject.AttachSprite(organTextures[organIndex], 50);
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
            obj.transform.localScale = new Vector3(organScale, organScale, organScale);
            obj.GetComponent<DragTag>().id = name;
            return obj.GetComponent<Organ>();
        }

        DragZone MakeDragZone(string name, Vector3 pos) {
            var obj = Instantiate(dragZonePrefab, pos, Quaternion.identity);
            obj.name = name;
            obj.transform.localScale = new Vector3(organScale, organScale, organScale);
            var zone = obj.GetComponent<DragZone>();
            zone.desiredObjectTag = name;
            return zone;
        }

        private void OnDrawGizmos() {
            // Draw lines representing drag zone closeness
            if (dragZones != null) {
                Gizmos.color = Color.green;
                foreach (DragZone zone in dragZones) {
                    foreach (DragZone innerZone in dragZones) {
                        if (innerZone == zone) continue;
                        Gizmos.color = Vector3.Distance(zone.transform.position, innerZone.transform.position) >= (organScale / 2) * Mathf.Sqrt(2) ? Color.green : Color.red;
                        Gizmos.DrawLine(zone.transform.position, innerZone.transform.position);
                    }
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
                MinigameCommon.DrawGizmoBox(
                    new Vector3(organPos.x + -0.5f, organPos.y + -maxSpawnedOrgans + 0.5f - (interOrganSpacing * (maxSpawnedOrgans - 1)), 0f),
                    new Vector3(organPos.x +  0.5f, organPos.y + -maxSpawnedOrgans + 0.5f - (interOrganSpacing * (maxSpawnedOrgans - 1)), 0f),
                    new Vector3(organPos.x +  0.5f, organPos.y +  0.5f,                    0f),
                    new Vector3(organPos.x + -0.5f, organPos.y +  0.5f,                    0f)
                );
                // Draw organs in spawn area
                for (int i = 0; i < maxSpawnedOrgans; i++) {
                    var pos = new Vector3(
                        organSpawnCenter.position.x,
                        organSpawnCenter.position.y - i - (interOrganSpacing * i),
                        organSpawnCenter.position.z
                    );
                    Gizmos.DrawWireSphere(pos, organScale / 2 / 2);
                }
            }

        }

    }
}
