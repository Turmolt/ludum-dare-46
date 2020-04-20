using System;
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

    public BabyAnimator BabyAnimator;

    public void Setup(int difficulty, List<GameObject> objects, Vector3 startPosition)
    {
        if (cam == null) cam = Camera.main;

        BabyAnimator.ToggleAnimation(false);
        this.difficulty = difficulty;
        BabyObject.OnDestinationReached = InstructBaby;
        dangerObjects = objects;
        BabyObject.StopMoving();
        BabyObject.transform.position = startPosition;
    }

    public void StartMoving()
    {
        BabyAnimator.ToggleAnimation(true);
        BabyObject.StartMoving(difficulty, RandomDangerousObjectPosition());
    }

    public void StopMoving()
    {
        BabyAnimator.ToggleAnimation(false);
        BabyObject.StopMoving();
    }

    void Update()
    {
        if(CloseToDanger()) OnDangerousObjectGrabbed?.Invoke();
    }

    public void InstructBaby()
    {
        BabyObject.SetTarget(RandomDangerousObjectPosition());
    }

    Vector3 RandomDangerousObjectPosition()
    {
        return dangerObjects[Random.Range(0, dangerObjects.Count)].transform.position.xy(BabyObject.transform.position.z);
    }

    bool CloseToDanger() => Physics.CheckSphere(BabyObject.transform.position, radius, LayerMask.GetMask("Targetables"));

    void OnDrawGizmos()
    {
        Gizmos.color = CloseToDanger() ? Color.red : Color.green;
        Gizmos.DrawWireSphere(BabyObject.transform.position,radius);
    }
}

