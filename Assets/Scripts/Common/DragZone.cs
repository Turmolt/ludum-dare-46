using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragZone : MonoBehaviour
{
    public string desiredObjectTag;

    private List<string> tags = new List<string>();

    [HideInInspector] bool hasDesiredObject = false;

    public Action<String> DesiredTagEntered;

    public Action<String> DesiredTagExited;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other) {
        var dragTag = other.GetComponent<DragTag>();
        if (dragTag == null) return;
        if (!tags.Contains(dragTag.id)) {
            tags.Add(dragTag.id);
        }

        hasDesiredObject = tags.Contains(desiredObjectTag);
        if (dragTag.id == desiredObjectTag) {
            DesiredTagEntered?.Invoke(dragTag.id);
        }
    }

    private void OnTriggerExit(Collider other) {
        var dragTag = other.GetComponent<DragTag>();
        if (dragTag == null) return;
        if (tags.Contains(dragTag.id)) {
            tags.Remove(dragTag.id);
        }

        Debug.Log(desiredObjectTag);

        hasDesiredObject = tags.Contains(desiredObjectTag);
        if (dragTag.id == desiredObjectTag)
            DesiredTagExited?.Invoke(dragTag.id);
    }

    private void OnDrawGizmos() {
        Gizmos.color = hasDesiredObject ? new Color(0f, 1.0f, 0f, 0.5f) : new Color(1.0f, 0.0f, 0f, 0.5f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
