using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CenterMassRounder : MonoBehaviour
{
    [SerializeField] private Transform _massCenterMarker;
    [SerializeField] private float _markerMovementRadius;
    [SerializeField] private float _markerMovementAngularSpeed;

    private Rigidbody _rigidbody;
    private float _currentMarkerAnglePosition = 0;
    private float _markerMovementAngularSpeedInRads;
    private float _circleInRads;

    private void Awake()
    {
        _circleInRads = 2 * Mathf.PI;
        _rigidbody = GetComponent<Rigidbody>();
        _markerMovementAngularSpeedInRads = _markerMovementAngularSpeed * Mathf.Deg2Rad;

        if (_massCenterMarker.parent != transform)
            _massCenterMarker.SetParent(transform);
    }

    private void Update()
    {
        _currentMarkerAnglePosition += _markerMovementAngularSpeedInRads * Time.deltaTime;
        
        if (_currentMarkerAnglePosition > _circleInRads)
            _currentMarkerAnglePosition -= _circleInRads;
        else if (_currentMarkerAnglePosition < -_circleInRads)
            _currentMarkerAnglePosition += _circleInRads;

        float markerNewLocalPositionX = Mathf.Sin(_currentMarkerAnglePosition) * _markerMovementRadius;
        float markerNewLocalPositionZ = Mathf.Cos(_currentMarkerAnglePosition) * _markerMovementRadius;
        _massCenterMarker.localPosition = new Vector3(markerNewLocalPositionX, _massCenterMarker.localPosition.y, markerNewLocalPositionZ);
    }

    private void FixedUpdate()
    {
        _rigidbody.centerOfMass = _massCenterMarker.localPosition;
    }
}
