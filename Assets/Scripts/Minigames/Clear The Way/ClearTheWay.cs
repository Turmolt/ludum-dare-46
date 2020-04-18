using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CheeseTeam
{
    public class ClearTheWay : Minigame
    {

        public CrawlingBaby Baby;
        public GameObject[] DangerousObjectPrefabs;
        public GameObject DangerObjectParent;
        private List<GameObject> dangerousObjects = new List<GameObject>();

        private int baseNumber = 10;
        private float babyBuffer = 3.0f;

        private DraggableObject grabbedObject;

        private Camera cam;

        public override void Setup(int difficulty)
        {
            if (cam == null) cam = Camera.main;
            
            SpawnObjects();
            Baby.Setup(difficulty, dangerousObjects);
            base.Setup(difficulty);
        }


        void Update()
        {
            
            HandleMouse();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Setup(1);
            }

        }

        void HandleMouse()
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

        void SpawnObjects()
        {
            int numberOfObjects = (int)(baseNumber + (difficulty / 3.0f)), num = dangerousObjects.Count, difference = numberOfObjects- dangerousObjects.Count;
        
            for (int i = 0; i < numberOfObjects; i++)
            {
                var needAdditional = (i < difference);
                GameObject newObject = needAdditional ? Instantiate(DangerousObjectPrefabs[Random.Range(0, DangerousObjectPrefabs.Length)], DangerObjectParent.transform) : dangerousObjects[i];
                newObject.transform.position = RandomDangerObjectPosition(i);
                newObject.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                if(needAdditional) dangerousObjects.Add(newObject);
            }
        
        }

        Vector3 RandomDangerObjectPosition(int n)
        {
            var point = MinigameCommon.RandomPointOnScreen(cam, 0f);
            point.z = Baby.BabyObject.transform.position.z;
            var moveAwayDist = babyBuffer - Vector3.Distance(Baby.BabyObject.transform.position, point);
            if (moveAwayDist > 0f)
            {
                var movement = Vector3.Normalize(Baby.BabyObject.transform.position - point) * (moveAwayDist + Random.Range(0f, 2f));
                point -= movement;
            }
            return point;
        }

    }

}