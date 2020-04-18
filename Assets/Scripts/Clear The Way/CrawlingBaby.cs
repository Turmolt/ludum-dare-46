using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlingBaby : MonoBehaviour
{

    public MovingObject BabyObject;

    private int difficulty = 1;

    public float Radius = 3f;

    public void Setup()
    {
        BabyObject.ResetPosition();
        transform.eulerAngles = transform.eulerAngles.xy(Random.Range(0f, 360f));
        BabyObject.StartMoving(difficulty++);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Setup();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(BabyObject.transform.position,Radius);
    }
}
