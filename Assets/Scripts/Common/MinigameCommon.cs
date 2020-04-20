using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MinigameCommon : MonoBehaviour
{
    public static Vector3 RandomPointOnScreen(Camera cam, float edgePadding)
    {
        var vector = new Vector3(
            Random.Range(edgePadding, 1.0f - edgePadding) * Screen.width, 
            Random.Range(edgePadding, 1.0f - edgePadding) * Screen.height, 
            1f
        );
        return cam.ScreenToWorldPoint(vector);
    }

    public static Vector3 RandomPointOnXYPlane(Vector3 pos, Vector2 range, float edgePadding) {
        var offX = (range.x) - edgePadding;
        var offY = (range.y) - edgePadding;
        return new Vector3(
            pos.x + Random.Range(-offX, offX),
            pos.y + Random.Range(-offY, offY),
            0f
        );
    }


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

    public static void DrawGizmoBox(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4) {
        Gizmos.DrawLine(
            point1,
            point2
        );
        Gizmos.DrawLine(
            point2,
            point3
        );
        Gizmos.DrawLine(
            point3,
            point4
        );
        Gizmos.DrawLine(
            point4,
            point1
        );
    }
}
