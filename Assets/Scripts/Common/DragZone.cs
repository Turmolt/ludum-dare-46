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
        if (!tags.Contains(other.tag)) {
            tags.Add(other.tag);
        }

        hasDesiredObject = tags.Contains(desiredObjectTag);

        if (other.tag == desiredObjectTag)
            DesiredTagEntered(other.tag);
    }

    private void OnTriggerExit(Collider other) {
        if (tags.Contains(other.tag)) {
            tags.Remove(other.tag);
        }

        hasDesiredObject = tags.Contains(desiredObjectTag);

        if (other.tag == desiredObjectTag)
            DesiredTagExited(other.tag);
    }
}
