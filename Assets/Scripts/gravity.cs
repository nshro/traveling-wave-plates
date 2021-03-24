using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravity : MonoBehaviour 
{

    void FixedUpdate()
    {
        var rb = GetComponent<MyRigidbody>();
        rb.AddForce(new Vector3(0f, -9.8f, 0f));
    }
}
