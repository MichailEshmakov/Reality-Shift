using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallerByTouch : MonoBehaviour
{
    private List<KinematicChanger> _kinematicChangers;

    private void Awake()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        _kinematicChangers = new List<KinematicChanger>(rigidbodies.Length);

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            KinematicChanger newKinematicChanger = rigidbody.gameObject.AddComponent(typeof(KinematicChanger)) as KinematicChanger;
            _kinematicChangers.Add(newKinematicChanger);
            newKinematicChanger.TrySetKinematic(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Ball ball))
        {
            foreach (KinematicChanger kinematicChanger in _kinematicChangers)
            {
                kinematicChanger.TrySetKinematic(false);
            }
        }
    }
}
