using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

sealed public class Text : ButtonAndTextBase
{
    [SerializeField] private TextMeshPro text;

    void Awake()
    {
        if (text) text.color = new Color(1, 1, 1, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        if (text) text.color = new Color(1, 1, 1, alpha);
    }
}
