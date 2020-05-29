using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 500f * Time.deltaTime);
    }

}
