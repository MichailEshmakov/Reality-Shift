using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] Transform _camera;
    [SerializeField] Transform _player;

    private Vector3 _startOffset;
    private Quaternion _startRotation;
    private Vector3 _resultAdditivePosition;
    private Quaternion _resultAdditiveRotation;
    private Vector3 _previousCameraPosition;
    private Quaternion _previousCameraRotation;

    public Vector3 StartOffset => _startOffset;
    public Quaternion StartRotation => _startRotation;
    public Vector3 PreviousCameraPosition => _previousCameraPosition;
    public Quaternion PreviousCameraRotation => _previousCameraRotation;

    void Start()
    {
        _startRotation = _camera.rotation;
        _startOffset = _camera.position - _player.position;

        Reset();
    }

    private void LateUpdate()
    {
        _camera.position += _resultAdditivePosition;
        _camera.rotation = _resultAdditiveRotation * _camera.rotation;

        Reset();
    }

    public void AddPosition(Vector3 additivePosition)
    {
        _resultAdditivePosition += additivePosition;
    }

    public void AddRotation(Quaternion additiveRotation)
    {
        _resultAdditiveRotation = additiveRotation * _resultAdditiveRotation;
    }

    private void Reset()
    {
        _resultAdditivePosition = Vector3.zero;
        _resultAdditiveRotation = Quaternion.Euler(Vector3.zero);
        _previousCameraPosition = _camera.position;
        _previousCameraRotation = _camera.rotation;
    }
}
