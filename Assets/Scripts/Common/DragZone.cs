using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragZone : MonoBehaviour
{
    public string desiredObjectTag;

    private List<string> tags = new List<string>();

    [HideInInspector] public bool hasDesiredObject = false;

    public Action<String> DesiredTagEntered;

    public Action<String> DesiredTagExited;

    Collider coll;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other) {
        var dragTag = other.GetComponent<DragTag>();
        if (dragTag == null) return;
        // if (!tags.Contains(dragTag.id)) {
            tags.Add(dragTag.id);
        // }

        hasDesiredObject = tags.Contains(desiredObjectTag);
        if (dragTag.id == desiredObjectTag) {
            DesiredTagEntered?.Invoke(dragTag.id);
        }
    }

    private void OnTriggerExit(Collider other) {
        var dragTag = other.GetComponent<DragTag>();
        if (dragTag == null) return;
        // if (tags.Contains(dragTag.id)) {
            tags.Remove(dragTag.id);
        // }

        hasDesiredObject = tags.Contains(desiredObjectTag);
        if (dragTag.id == desiredObjectTag)
            DesiredTagExited?.Invoke(dragTag.id);
    }

    private void OnDrawGizmos() {
        Gizmos.color = hasDesiredObject ? new Color(0f, 1.0f, 0f, 0.5f) : new Color(1.0f, 0.0f, 0f, 0.5f);
        Vector2 bounds = new Vector2(coll.bounds.size.x / 2, coll.bounds.size.y / 2);
        var zonePos = transform.position;
        MinigameCommon.DrawGizmoBox(
                new Vector3(zonePos.x + -bounds.x, zonePos.y + -bounds.y, 0f),
                new Vector3(zonePos.x +  bounds.x, zonePos.y + -bounds.y, 0f),
                new Vector3(zonePos.x +  bounds.x, zonePos.y +  bounds.y, 0f),
                new Vector3(zonePos.x + -bounds.x, zonePos.y +  bounds.y, 0f)
            );
    }
}
