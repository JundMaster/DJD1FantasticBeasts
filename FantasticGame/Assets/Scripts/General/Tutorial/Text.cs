using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

sealed public class Text : ButtonAndTextBase
{
    [SerializeField] private TextMeshPro text;

    void Awake()
    {
        if (text) text.color = new Color(1, 1, 1, base.alpha);
    }

    void Update()
    {
        // Sets text color alpha to alpha from base
        if (text) text.color = new Color(1, 1, 1, base.alpha);
    }
}
