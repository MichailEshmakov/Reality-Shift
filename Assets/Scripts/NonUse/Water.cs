using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private float _force;

    private Dictionary<GameObject, Rigidbody> _swimmingRigidBodies;

    private void Awake()
    {
        _swimmingRigidBodies = new Dictionary<GameObject, Rigidbody>();
    }

    private void FixedUpdate()
    {
        foreach (Rigidbody rigidbody in _swimmingRigidBodies.Values)
        {
            rigidbody.AddForce(Vector3.up * _force * Time.deltaTime, ForceMode.Force);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_swimmingRigidBodies.ContainsKey(other.gameObject) == false)
        {
            Rigidbody rigidbody = other.gameObject.GetComponentInParent<Rigidbody>();
            if (rigidbody != null)
                _swimmingRigidBodies.Add(other.gameObject, rigidbody);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_swimmingRigidBodies.ContainsKey(other.gameObject))
        {
            _swimmingRigidBodies.Remove(other.gameObject);
        }
    }
}
