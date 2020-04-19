using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CheeseTeam{
    
    public class OperationMinigame : Minigame {

        public GameObject organPrefab;
        private DraggableObject grabbedObject;

        private List<Organ> organs = new List<Organ>();

        public override void StartGame() {
            for (int i = 0; i < 5; i++) {
                var organ = MakeOrgan("Organ " + i.ToString(), Vector3.zero);
                organs.Add(organ);
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
        }

        Organ MakeOrgan(string name, Vector3 pos) {
            var obj = Instantiate(organPrefab, pos, Quaternion.identity);
            obj.name = name;
            obj.GetComponent<DragTag>().id = name;
            return obj.GetComponent<Organ>();
        }

    }

}
