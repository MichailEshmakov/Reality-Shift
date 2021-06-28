using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrillEffect : Effect
{
    [SerializeField] float _maxAngle;
    [SerializeField] float _rotationSpeed;
    [SerializeField] CameraMover _cameraMover;
    [SerializeField] Transform _camera;
    [SerializeField] Transform _player;

    private float _currentAngle;
    private int _directionCoefficient = 1;

    private void Update()
    {
        _currentAngle += _rotationSpeed * Time.deltaTime * _directionCoefficient;
        //Quaternion newRotation = Quaternion.AngleAxis(_rotationSpeed * Time.deltaTime * _directionCoefficient, _camera.forward) * _camera.rotation;
        //_cameraMover.AddRotation(newRotation * Quaternion.Inverse(_cameraMover.PreviousCameraRotation));// Это работает, но не с удочерением

        _cameraMover.AddRotation(Quaternion.AngleAxis(_rotationSpeed * Time.deltaTime * _directionCoefficient, _camera.forward));

        if (_directionCoefficient >= 0 && _currentAngle >= _maxAngle)
        {
            _directionCoefficient = -1;
        }
        else if (_directionCoefficient < 0 && _currentAngle <= -_maxAngle)
        {
            _directionCoefficient = 1;
        }
    }
}
