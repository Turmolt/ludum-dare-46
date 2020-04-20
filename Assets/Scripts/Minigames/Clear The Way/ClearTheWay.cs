using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CheeseTeam
{
    public class ClearTheWay : Minigame
    {

        public CrawlingBaby Baby;
        public GameObject[] DangerousObjectPrefabs;
        public GameObject DangerObjectParent;
        private List<GameObject> dangerousObjects = new List<GameObject>();

        private int baseNumber = 15;
        private float babyBuffer = 3.0f;

        private DraggableObject grabbedObject;

        private Camera cam;

        private bool isPlaying = false;

        public override bool Setup(int difficulty)
        {
            if (cam == null) cam = Camera.main;
            base.Setup(difficulty);

            isPlaying = false;
            Vector3 babyStart = MinigameCommon.RandomPointOnScreen(cam, .25f).xy(Baby.BabyObject.transform.position.z);
            SpawnObjects(babyStart);
            Baby.Setup(difficulty, dangerousObjects, babyStart);
            Baby.OnDangerousObjectGrabbed = GameLost;
            return true;
        }

        void GameLost()
        {
            if (!isPlaying) return;
            isPlaying = false;
            Baby.StopMoving();
            OnGameLose?.Invoke();
        }

        public override void StartGame()
        {

            Baby.StartMoving();
            Invoke("EnablePlaying",.1f);
            base.StartGame();
        }

        void EnablePlaying() => isPlaying = true;

        void Update()
        {
            HandleMouse();
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
        
        void SpawnObjects(Vector3 babyStart)
        {
            int numberOfObjects = (int)(baseNumber + difficulty), difference = numberOfObjects- dangerousObjects.Count;
        
            for (int i = 0; i < baseNumber+difficulty/2; i++)
            {
                GameObject newObject = Instantiate(DangerousObjectPrefabs[Random.Range(0, DangerousObjectPrefabs.Length)], DangerObjectParent.transform);
                newObject.transform.position = RandomDangerObjectPosition(babyStart);
                newObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                dangerousObjects.Add(newObject);
            }
        
        }

        Vector3 RandomDangerObjectPosition(Vector3 babyPosition)
        {
            var point = MinigameCommon.RandomPointOnScreen(cam, 0f);
            point.z = Baby.BabyObject.transform.position.z;
            var moveAwayDist = babyBuffer - Vector3.Distance(babyPosition, point);
            if (moveAwayDist > 0f)
            {
                var movement = Vector3.Normalize(babyPosition - point) * (moveAwayDist + Random.Range(0f, 2f));
                point -= movement;
                var screenPoint = cam.WorldToScreenPoint(point);

                if (screenPoint.x < 0 || screenPoint.y < 0 || screenPoint.x>Screen.width || screenPoint.y > Screen.height)
                {
                    //recalculate?
                    point = RandomDangerObjectPosition(babyPosition);
                }
            }
            return point;
        }

    }

}