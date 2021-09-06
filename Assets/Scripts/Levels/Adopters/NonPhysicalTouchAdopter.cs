using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPhysicalTouchAdopter : TouchAdopter
{
    private Dictionary<Transform, Transform> _previousParents;

    private void Awake()
    {
        _previousParents = new Dictionary<Transform, Transform>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsAdoptable(collision.transform))
        {
            if (_previousParents.ContainsKey(collision.transform) == false)
            {
                _previousParents.Add(collision.transform, collision.transform.parent);
                collision.transform.parent = transform;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (_previousParents.ContainsKey(collision.transform))
        {
            collision.transform.parent = _previousParents[collision.transform];
            _previousParents.Remove(collision.transform);
        }
    }
}
