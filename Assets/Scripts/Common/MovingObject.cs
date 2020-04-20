using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class MovingObject : MonoBehaviour
{
    private Vector3 targetPosition;

    public float BaseSpeed = 2.0f;

    private float speed;

    private bool moving = false;

    public Action OnReachedEnd;

    private Camera cam;

    public Action OnDestinationReached;

    void Start()
    {
        cam = Camera.main;
    }

    public void StartMoving(int difficulty, Vector3 target)
    {
        targetPosition = target;
        moving = true;
        speed = Mathf.Clamp(BaseSpeed+(.1f*difficulty), BaseSpeed,BaseSpeed*2f);
    }

    public void SetTarget(Vector3 target) => targetPosition = target;
    

    public void StopMoving() => moving = false;
    

    public void Update()
    {
        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            var dir = transform.position - targetPosition;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            //transform.eulerAngles = transform.eulerAngles.z();
            if (transform.position == targetPosition)
            {
                if (OnDestinationReached == null) moving = false;
                else OnDestinationReached.Invoke();
            }
        }
    }
}
