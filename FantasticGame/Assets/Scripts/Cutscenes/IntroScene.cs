using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class IntroScene : MonoBehaviour
{
    public static bool INTROSCENE { get; private set; } = false;

    void Awake()
    {
        INTROSCENE = true;
    }


    void Update()
    {
        
    }
}
