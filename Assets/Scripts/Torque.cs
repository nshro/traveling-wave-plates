using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torque : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            var rb = GetComponent<Rigidbody>();
            var omega = new Vector3(0f, 1f, 0f);
            var Id = rb.inertiaTensor;
            var Ir = rb.inertiaTensorRotation;
            var IrI = Quaternion.Inverse(Ir);
            // T=I*omega
            // T=Ir*Id*IrI*omega
            var torque = Ir * Vector3.Scale(Id, IrI*omega);
            rb.AddTorque(torque, ForceMode.Impulse);
        }
    }
}
