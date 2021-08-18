using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrillEffect : CameraMovingEffect
{
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _camera;

    private float _currentAngle;
    private int _directionCoefficient = 1;

    private void Update()
    {
        _currentAngle += _rotationSpeed * Time.deltaTime * _directionCoefficient;
        CameraMover.AddRotation(Quaternion.AngleAxis(_rotationSpeed * Time.deltaTime * _directionCoefficient, _camera.forward));

        if (_directionCoefficient >= 0 && _currentAngle >= _maxAngle)
        {
            _directionCoefficient = -1;
        }
        else if (_directionCoefficient < 0 && _currentAngle <= -_maxAngle)
        {
            _directionCoefficient = 1;
        }
    }

    protected override void ResetParameters()
    {
        _currentAngle = 0;
    }
}
