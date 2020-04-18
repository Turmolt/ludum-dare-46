using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    [HideInInspector] public bool InHand;

    public bool lockX;
    public bool lockY;
    public bool lockZ;

    private Camera cam;
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (InHand)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition(), Speed());
        }
    }

    private float Speed()
    {
        float speed = 5.0f;

        return speed * Vector3.Distance(Input.mousePosition, TargetPosition());
    }

    private Vector3 TargetPosition()
    {
        var worldMouse = cam.ScreenToWorldPoint(ClampedMouse());
        return new Vector3(lockX ? transform.position.x : worldMouse.x, lockY ? transform.position.y : worldMouse.y, lockZ ? transform.position.z : worldMouse.z);
    }

    Vector3 ClampedMouse()
    {
        var mousePos = Input.mousePosition;
        return new Vector3(Mathf.Clamp(mousePos.x,0,Screen.width), Mathf.Clamp(mousePos.y, 0, Screen.height), 1f);
    }
}