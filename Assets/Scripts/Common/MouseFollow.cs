using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        transform.position = TargetPosition();
    }

    Vector3 ClampedMouse()
    {
        var mousePos = Input.mousePosition;
        return new Vector3(Mathf.Clamp(mousePos.x,0,Screen.width), Mathf.Clamp(mousePos.y, 0, Screen.height), 1f);
    }

    private Vector3 TargetPosition()
    {
        var worldMouse = Camera.main.ScreenToWorldPoint(ClampedMouse());
        return new Vector3(worldMouse.x, worldMouse.y, transform.position.z);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.125f, 0.125f, 0.125f));
    }
}
