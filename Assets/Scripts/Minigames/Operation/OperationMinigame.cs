using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CheeseTeam {
    public class OperationMinigame : Minigame {

        private bool isPlaying = true;

        public int maxSpawnedOrgans = 5;
        public float organScale = 1.0f;
        public float interOrganSpacing = 0.125f;

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


        public override void Setup(int difficulty) {
            base.Setup(difficulty);
            organs = new List<Organ>();
            dragZones = new List<DragZone>();
        }

        public override void StartGame() {
            base.StartGame();

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
                while (true) {
                    var hasCollision = false;
                    foreach (var zone in dragZones) {
                        if (Vector3.Distance(zone.transform.position, pos) < 1.414f) {
                            hasCollision = true;
                        }
                    }
                    if (hasCollision) {
                        pos = MinigameCommon.RandomPointOnXYPlane(dragZoneSpawnCenter.position, dragZoneSpawnRange, 1f);
                    } else {
                        break;
                    }
                }
                var dragZone = MakeDragZone(desiredTag, pos);
                dragZone.gameObject.AttachSprite(dragZoneTextures[organIndex], 40);
                dragZones.Add(dragZone);
            }
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

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;

            // Draw drag zone spawn area
            if (dragZoneSpawnCenter == null || dragZoneSpawnRange == null)
                return;
            var zonePos = dragZoneSpawnCenter.position;
            MinigameCommon.DrawGizmoBox(
                new Vector3(zonePos.x + -dragZoneSpawnRange.x, zonePos.y + -dragZoneSpawnRange.y, 0f),
                new Vector3(zonePos.x +  dragZoneSpawnRange.x, zonePos.y + -dragZoneSpawnRange.y, 0f),
                new Vector3(zonePos.x +  dragZoneSpawnRange.x, zonePos.y +  dragZoneSpawnRange.y, 0f),
                new Vector3(zonePos.x + -dragZoneSpawnRange.x, zonePos.y +  dragZoneSpawnRange.y, 0f)
            );

            // Draw organ spawn area
            if (organSpawnCenter == null)
                return;
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
                Gizmos.DrawWireSphere(pos, organScale / 2);
            }

        }

    }
}
