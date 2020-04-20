using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyAnimator : MonoBehaviour
{
    public Transform[] RightAppendages;
    private Vector3[] rightStartRotations;
    public Transform[] LeftAppendages;
    private Vector3[] leftStartRotations;

    private float maxDelta = 15f;
    private float speed = 10.0f;
    private bool isPlaying;

    void Awake()
    {
        rightStartRotations=new Vector3[RightAppendages.Length];
        leftStartRotations=new Vector3[LeftAppendages.Length];
        for (int i = 0; i < RightAppendages.Length; i++)
        {
            rightStartRotations[i] = RightAppendages[i].localEulerAngles;
            leftStartRotations[i] = LeftAppendages[i].localEulerAngles;
        }
    }

    public void ToggleAnimation(bool endState)
    {
        isPlaying = endState;
    }
    void Update()
    {
        if (isPlaying)
        {
            for (int i = 0; i < RightAppendages.Length; i++)
            {
                RightAppendages[i].localEulerAngles =
                    rightStartRotations[i].xy(rightStartRotations[i].z + Mathf.Sin(Time.time*speed-i*2.0f) * maxDelta);
                LeftAppendages[i].localEulerAngles =
                    leftStartRotations[i].xy(leftStartRotations[i].z + Mathf.Sin(Time.time*speed-10-i*3.0f) * maxDelta);
            }
        }
    }

}
