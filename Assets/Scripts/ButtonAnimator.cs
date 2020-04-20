using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimator : MonoBehaviour
{
    private float rotation = 10f;
    private float interval = 1.0f;
    private float runtime = 0f;
    private Vector3 StartRotation;
    private bool negative;

    void Start()
    {
        StartRotation = transform.eulerAngles;
        //start in a positive angle, negative
        transform.eulerAngles = StartRotation.xy(StartRotation.z - rotation);
    }

    void Update()
    {
        runtime += Time.deltaTime;
        if (runtime > interval)
        {
            runtime = 0f;
            transform.eulerAngles = StartRotation.xy(StartRotation.z + (negative ? -rotation : rotation));
            negative = !negative;
        }
    }
}
