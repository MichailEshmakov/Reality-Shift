using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicPatrol : Patrol
{
    [SerializeField] private float _accceleration;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 neededVelocity = (TargetPoint.transform.position - transform.position).normalized * Speed;
        _rigidbody.velocity = Vector3.MoveTowards(_rigidbody.velocity, neededVelocity, _accceleration * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MovePoint movePoint) && movePoint == TargetPoint)
        {
            SetNextTargetPoint();
        }
    }
}
