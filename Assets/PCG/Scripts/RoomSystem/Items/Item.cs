using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private BoxCollider2D itemCollider;

    public void Initialize(ItemData itemData)
    {
        this.gameObject.layer = 11;
        //set sprite
        spriteRenderer.sprite = itemData.sprite;
        //set sprite offset
        spriteRenderer.transform.localPosition = new Vector2(0.5f * itemData.size.x, 0.5f * itemData.size.y);
        if (itemData.isCollide == true)
        {
            if (itemData.isPartialCollide == true)
            {
                itemCollider.size = new Vector2Int(itemData.size.x, 1);
                itemCollider.offset = spriteRenderer.transform.localPosition - new Vector3(0, itemData.size.y / 2);
            }
            else
            {
                itemCollider.size = itemData.size;
                itemCollider.offset = spriteRenderer.transform.localPosition;
            }
        }
        else
        {
            itemCollider.size = new Vector2(0, 0);
        }

    }

}

