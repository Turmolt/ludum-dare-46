﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrawlingBaby : MonoBehaviour
{
    public MovingObject BabyObject;

    private List<GameObject> dangerObjects;

    private float radius = .75f;

    private int difficulty;

    public Action OnDangerousObjectGrabbed;

    private Camera cam;

    private bool isPlaying = false;

    public void Setup(int difficulty, List<GameObject> objects, Vector3 startPosition)
    {
        if (cam == null) cam = Camera.main;
        isPlaying = false;
        this.difficulty = difficulty;
        BabyObject.OnDestinationReached = InstructBaby;
        dangerObjects = objects;
        BabyObject.StopMoving();
        BabyObject.transform.position = startPosition;
    }

    public void StartMoving()
    {
        isPlaying = true;
        BabyObject.StartMoving(difficulty, RandomDangerousObjectPosition());
    }


    void Update()
    {
        if(CloseToDanger()&&isPlaying) OnDangerousObjectGrabbed?.Invoke();
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

