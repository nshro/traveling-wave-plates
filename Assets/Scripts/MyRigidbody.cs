using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRigidbody : MonoBehaviour
{
    Vector3 accelaration;
    Vector3 velocity;
    Vector3 position;
    const float dt = 1f/60f;

    void Start()
    {
        position = transform.position;
    }

    public void AddForce(Vector3 force) {
        accelaration += force;
    }

    void FixedUpdate()
    {
        velocity += accelaration * dt;
        position += velocity * dt;
        if (position.y < 0.5f) {
            velocity = -velocity;
        }
        transform.position = position;
        accelaration = Vector3.zero;
    }


}
