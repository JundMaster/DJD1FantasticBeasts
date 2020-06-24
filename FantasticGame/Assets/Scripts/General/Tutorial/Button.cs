using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class Button : ButtonAndTextBase
{
    [SerializeField] private SpriteRenderer sprite;

    void Awake()
    {
        if (sprite) sprite.color = new Color(1, 1, 1, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        if (sprite) sprite.color = new Color(1, 1, 1, alpha);
    }
}
