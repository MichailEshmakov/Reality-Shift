using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShiftEffect : CameraTransformingEffect, ICameraMovingAdder
{
    [SerializeField] private float _maxShift;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _acceleration;

    private float _currentShift;
    private Vector3 _shiftDirection;
    private bool _isIncreasingShift;
    private float _breakingDistance;
    private float _currentSpeed;

    private void Awake()
    {
        _breakingDistance = ComputeBrakingDistance();
    }

    private void LateUpdate()
    {
        if (_currentSpeed > _maxSpeed)
            _currentSpeed = _maxSpeed;

        float adding = ComputeMovementAdding();
        CameraMover.AddPosition(_shiftDirection * adding, this);
        _currentShift += adding;
        TryChangeSpeed();
        TryToggleIncreasingShiftFlag();
    }

    private float ComputeMovementAdding()
    {
        return Mathf.MoveTowards(_currentShift, _isIncreasingShift ? _maxShift : 0, _currentSpeed * Time.deltaTime) - _currentShift;
    }

    private void TryChangeSpeed()
    {
        if (_currentShift < _breakingDistance)
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, _isIncreasingShift ? _maxSpeed : 0, _acceleration * Time.deltaTime);
        else if (_maxShift - _currentShift < _breakingDistance)
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, _isIncreasingShift ? 0 : _maxSpeed, _acceleration * Time.deltaTime);
    }

    private float ComputeBrakingDistance()
    {
        if (_acceleration != 0)
        {
            float breakingDistance = (_maxSpeed * _maxSpeed) / (2 *_acceleration);
            float halfFullDistance = _maxShift / 2;
            if (breakingDistance > halfFullDistance)
                breakingDistance = halfFullDistance;

            return breakingDistance;
        }
        else
            return 0;
    }    

    private void TryToggleIncreasingShiftFlag()
    {
        if (_isIncreasingShift && (_currentShift >= _maxShift || _currentSpeed == 0))
        {
            _isIncreasingShift = false;
        }
        else if (_isIncreasingShift == false && (_currentShift <= 0 || _currentSpeed == 0))
        {
            _isIncreasingShift = true;
            SetDirection();
        }
    }

    protected override void ResetParameters()
    {
        _currentShift = 0;
        _currentSpeed = 0;
        _isIncreasingShift = true;
        SetDirection();
    }

    private void SetDirection()
    {
        _shiftDirection = Random.onUnitSphere;
    }
}
