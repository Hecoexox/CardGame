using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public void Setup(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
