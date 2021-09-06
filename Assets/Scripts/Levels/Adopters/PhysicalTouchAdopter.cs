using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicalTouchAdopter : TouchAdopter
{
    private List<Rigidbody> _adoptedRigidbodies;
    private Vector3 _previousPosition;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _adoptedRigidbodies = new List<Rigidbody>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        foreach (Rigidbody rigidbody in _adoptedRigidbodies)
        {
            rigidbody.velocity += transform.position - _previousPosition;
        }

        _previousPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsAdoptable(collision.transform))
        {
            if (collision.gameObject.TryGetComponent(out Rigidbody rigidbody))
            {
                if (_adoptedRigidbodies.Contains(rigidbody) == false)
                    _adoptedRigidbodies.Add(rigidbody);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody rigidbody))
        {
            if (_adoptedRigidbodies.Contains(rigidbody))
                _adoptedRigidbodies.Remove(rigidbody);
        }
    }
}
