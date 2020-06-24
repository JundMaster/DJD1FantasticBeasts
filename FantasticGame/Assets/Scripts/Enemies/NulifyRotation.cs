using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NulifyRotation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
