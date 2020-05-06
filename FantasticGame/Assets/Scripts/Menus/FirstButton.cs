using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstButton : MonoBehaviour
{
    [SerializeField] Button thisButton;
    
    // Start is called before the first frame update
    void Start()
    {
        thisButton.Select();
    }
}
