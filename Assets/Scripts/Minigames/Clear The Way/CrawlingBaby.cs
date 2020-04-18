using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlingBaby : MonoBehaviour
{
    public MovingObject BabyObject;

    private List<GameObject> dangerObjects;

    private float radius = .75f;

    public void Setup(int difficulty, List<GameObject> objects)
    {
        BabyObject.OnDestinationReached = InstructBaby;
        dangerObjects = objects;
        BabyObject.ResetPosition();
        transform.eulerAngles = transform.eulerAngles.xy(Random.Range(0f, 360f));
        BabyObject.StartMoving(difficulty, RandomDangerousObjectPosition());
    }

    public void InstructBaby()
    {
        BabyObject.SetTarget(RandomDangerousObjectPosition());
    }

    Vector3 RandomDangerousObjectPosition()
    {
        return dangerObjects[Random.Range(0, dangerObjects.Count)].transform.position.xy(BabyObject.transform.position.z);
    }

    bool CloseToDanger() => Physics.CheckSphere(BabyObject.transform.position, radius, LayerMask.GetMask("Interactables"));

    void OnDrawGizmos()
    {
        Gizmos.color = CloseToDanger() ? Color.red : Color.green;
        Gizmos.DrawWireSphere(BabyObject.transform.position,radius);
    }
}

