using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform _master;

    private Vector3 _offset;

    private void Start()
    {
        if (_master != null)
            _offset = transform.position - _master.position;
    }

    private void Update()
    {
        if (_master != null)
            transform.position = _master.position + _offset;
    }

    public void Init(Transform master)
    {
        _master = master;
    }
}
