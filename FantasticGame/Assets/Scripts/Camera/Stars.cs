using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class Stars : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(transform.position.x, 8f, transform.position.z);
    }
}
