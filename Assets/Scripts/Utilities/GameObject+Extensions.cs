using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static SpriteRenderer AttachSprite(this GameObject obj, Sprite sprite, float height) {
        var spriteHolder = new GameObject("Sprite Holder");
        var spriteRenderer = spriteHolder.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        spriteHolder.transform.parent = obj.transform;
        spriteHolder.transform.localPosition = new Vector3(0f, 0f, -height);
        spriteHolder.transform.localRotation = Quaternion.identity;

        return spriteRenderer;
    }
}
