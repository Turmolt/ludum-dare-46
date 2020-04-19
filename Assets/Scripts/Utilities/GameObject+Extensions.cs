using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static SpriteRenderer AttachSprite(this GameObject obj, Sprite sprite, int orderInLayer) {
        var spriteHolder = new GameObject("Sprite Holder");
        var spriteRenderer = spriteHolder.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = orderInLayer;

        spriteHolder.transform.parent = obj.transform;
        spriteHolder.transform.localPosition = new Vector3(0f, 0f, 0f);
        spriteHolder.transform.localRotation = Quaternion.identity;

        return spriteRenderer;
    }
}
