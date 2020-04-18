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

    void Start()
    {
        cam = Camera.main;
    }

    public void ResetPosition()
    {
        transform.position = Vector3.zero;
        SetRandomTarget();
        moving = false;
    }

    void SetRandomTarget()
    {
        targetPosition = cam.ScreenToWorldPoint(new Vector3(Random.Range(.1f, .9f) * Screen.width, Random.Range(.1f, .9f) * Screen.height, 1f)).xy(transform.position.z);
    }

    public void StartMoving(int difficulty)
    {
        moving = true;
        speed = Mathf.Clamp(BaseSpeed+(.1f*difficulty), BaseSpeed,BaseSpeed*2f);
    }

    public void Update()
    {
        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            transform.LookAt(targetPosition);
            if (transform.position == targetPosition)
            {
                SetRandomTarget();
            }
        }
    }
}
