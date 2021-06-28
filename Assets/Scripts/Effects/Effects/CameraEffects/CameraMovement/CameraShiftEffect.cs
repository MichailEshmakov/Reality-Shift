using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShiftEffect : CameraMovingEffect
{
    [SerializeField] private float _maxShift;
    [SerializeField] private float speed;

    private float _currentShift;
    private Vector3 _shiftDirection;
    private bool _isIncreasingShift;

    private void Update()
    {
        float adding = Mathf.MoveTowards(_currentShift, _isIncreasingShift ? _maxShift : 0, speed * Time.deltaTime) - _currentShift; 
        CameraMover.AddPosition(_shiftDirection * adding);
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

    protected override void Reset()
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
