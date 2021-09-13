using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Patrol : MonoBehaviour
{
    [SerializeField] private MovePoint[] _movePoints;
    [SerializeField] private float _speed;

    private int _targetPointIndex = 0;

    protected MovePoint TargetPoint => _movePoints.Length > 0 ? _movePoints[_targetPointIndex] : null;
    protected float Speed => _speed;

    protected void SetNextTargetPoint()
    {
        if (_targetPointIndex < _movePoints.Length - 1)
            _targetPointIndex++;
        else
            _targetPointIndex = 0;
    }
}
