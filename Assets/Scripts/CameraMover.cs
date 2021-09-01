using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private Camera _mainCamera;

    private Vector3 _startOffset;
    private Quaternion _startRotation;
    private Vector3 _resultAdditivePosition;
    private Quaternion _resultAdditiveRotation;
    private Vector3 _previousCameraPosition;
    private Quaternion _previousCameraRotation;
    private Quaternion _previousBallRotation;
    private Vector3 _previousBallPosition;
    private bool _isStartParametersSet;

    public Vector3 StartOffset => _startOffset;
    public Quaternion StartRotation => _startRotation;
    public Vector3 PreviousCameraPosition => _previousCameraPosition;
    public Quaternion PreviousCameraRotation => _previousCameraRotation;
    public Quaternion PreviousBallRotation => _previousBallRotation;
    public Vector3 PreviousBallPosition => _previousBallPosition;
    public bool IsStartParametersSet => _isStartParametersSet;

    public event UnityAction CameraMovementEffectDisabled;
    public event UnityAction StartParametersSet;

    private void Awake()
    {
        ResetParameters();
        _startRotation = _mainCamera.transform.rotation;
        _startOffset = _mainCamera.transform.position - _ball.transform.position;
        _isStartParametersSet = true;
        StartParametersSet?.Invoke();
    }

    private void LateUpdate()
    {
        _mainCamera.transform.position += _resultAdditivePosition;
        _mainCamera.transform.rotation = _resultAdditiveRotation * _mainCamera.transform.rotation;

        ResetParameters();
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
            ResetCameraPosition();
            effect.Disabled -= OnCameraMovementEffectDisabled;
            ResetParameters();
            CameraMovementEffectDisabled?.Invoke();
        }
    }

    private void ResetParameters()
    {
        _resultAdditivePosition = Vector3.zero;
        _resultAdditiveRotation = Quaternion.Euler(Vector3.zero);

        if (_mainCamera != null)
        {
            _previousCameraPosition = _mainCamera.transform.position;
            _previousCameraRotation = _mainCamera.transform.rotation;
        }

        if (_ball != null)
        {
            _previousBallRotation = _ball.transform.rotation;
            _previousBallPosition = _ball.transform.position;
        }
    }

    private void ResetCameraPosition()
    {
        if (_mainCamera != null)
        {
            _mainCamera.transform.rotation = _startRotation;
            if (_ball != null)
                _mainCamera.transform.position = _ball.transform.position + _startOffset;
        }
    }
}
