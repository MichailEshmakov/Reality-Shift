using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TouchAdopter : MonoBehaviour
{
    [SerializeField] private List<Transform> _nonAdoptables;

    protected bool IsAdoptable(Transform transform)
    {
        return _nonAdoptables.Contains(transform) == false;
    }
}
