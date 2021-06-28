using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _player;

    private Vector3 _startOffset;
    private Quaternion _startRotation;
    private Vector3 _resultAdditivePosition;
    private Quaternion _resultAdditiveRotation;
    private Vector3 _previousCameraPosition;
    private Quaternion _previousCameraRotation;
    private Quaternion _previousPlayerRotation;
    private Vector3 _previousPlayerPosition;

    public Vector3 StartOffset => _startOffset;
    public Quaternion StartRotation => _startRotation;
    public Vector3 PreviousCameraPosition => _previousCameraPosition;
    public Quaternion PreviousCameraRotation => _previousCameraRotation;
    public Quaternion PreviousPlayerRotation => _previousPlayerRotation;
    public Vector3 PreviousPlayerPosition => _previousPlayerPosition;

    public event UnityAction CameraMovementEffectDisabled;

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

    public void SubscribeCameraMovementEffect(CameraMovingEffect effect)
    {
        effect.Disabled += OnCameraMovementEffectDisabled;
    }

    private void OnCameraMovementEffectDisabled(Effect effect)
    {
        if (effect is CameraMovingEffect)
        {
            if (_camera != null)
            {
                _camera.rotation = _startRotation;
                if (_player != null)
                    _camera.position = _player.position + _startOffset;
            }

            effect.Disabled -= OnCameraMovementEffectDisabled;
            Reset();
            CameraMovementEffectDisabled?.Invoke();
        }
    }

    private void Reset()
    {
        _resultAdditivePosition = Vector3.zero;
        _resultAdditiveRotation = Quaternion.Euler(Vector3.zero);

        if (_camera != null)
        {
            _previousCameraPosition = _camera.position;
            _previousCameraRotation = _camera.rotation;
        }

        if (_player != null)
        {
            _previousPlayerRotation = _player.rotation;
            _previousPlayerPosition = _player.position;
        }
    }
}
