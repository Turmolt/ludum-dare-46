﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameCommon : MonoBehaviour
{
    public static GameObject RaycastFromMouseForTag(string tag, float maxDistance = 1000f)
    {
        Camera cam = Camera.main;
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(r, maxDistance: maxDistance, layerMask: LayerMask.GetMask(tag), hitInfo: out hit))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    public static object RaycastFromMouseForTag(string tag, Type seeking, float maxDistance = 1000f)
    {
        Camera cam = Camera.main;
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(r, maxDistance: maxDistance, layerMask: LayerMask.GetMask(tag), hitInfo: out hit))
        {
            return hit.collider.GetComponent(seeking);
        }
        return null;
    }

    public static GameObject RaycastFromMouse(float maxDistance = 1000f)
    {
        Camera cam = Camera.main;
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(r, maxDistance: maxDistance, hitInfo: out hit))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    public static object RaycastFromMouse(Type seeking, float maxDistance = 1000f)
    {
        Camera cam = Camera.main;
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(r, maxDistance: maxDistance, hitInfo: out hit))
        {
            return hit.collider.GetComponent(seeking);
        }
        return null;
    }
}
