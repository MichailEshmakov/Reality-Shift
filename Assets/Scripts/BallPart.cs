using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallPart : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void TakeForce(float breakingForce)
    {
        _rigidbody.AddForce(_rigidbody.centerOfMass.normalized * breakingForce, ForceMode.Impulse);
    }
}
