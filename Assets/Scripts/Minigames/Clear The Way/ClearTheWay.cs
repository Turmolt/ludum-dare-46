using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CheeseTeam
{
    public class ClearTheWay : Minigame
    {
        private DraggableObject grabbedObject;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                grabbedObject = MinigameCommon.RaycastFromMouse(typeof(DraggableObject)) as DraggableObject;
                if (grabbedObject != null)
                {

                    grabbedObject.InHand = true;
                }
            }

            if (Input.GetMouseButtonUp(0) && grabbedObject != null)
            {
                grabbedObject.InHand = false;
                grabbedObject = null;

            }
        }

    }

}