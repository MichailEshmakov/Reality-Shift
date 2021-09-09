using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShiftEffect : CameraMovingEffect, ICameraMovingAdder
{
    [SerializeField] private float _maxShift;
    [SerializeField] private float _speed;

    private float _currentShift;
    private Vector3 _shiftDirection;
    private bool _isIncreasingShift;

    private void LateUpdate()
    {
        float adding = Mathf.MoveTowards(_currentShift, _isIncreasingShift ? _maxShift : 0, _speed * Time.deltaTime) - _currentShift;
        CameraMover.AddPosition(_shiftDirection * adding, this);
        _currentShift += adding;

        if (_isIncreasingShift && _currentShift >= _maxShift)
        {
            _isIncreasingShift = false;
        }
        else if (_isIncreasingShift == false && _currentShift <= 0)
        {
            _isIncreasingShift = true;
            SetDirection();
        }
    }

    protected override void ResetParameters()
    {
        _currentShift = 0;
        _isIncreasingShift = true;
        SetDirection();
    }

    private void SetDirection()
    {
        _shiftDirection = Random.onUnitSphere;
    }
}
