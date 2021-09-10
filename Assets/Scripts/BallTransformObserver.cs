using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTransformObserver : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private CameraMover _cameraMover;
    [SerializeField] private CameraRotator _cameraRotator;

    private Quaternion _previousBallRotation;
    private Vector3 _previousBallPosition;
    private bool _isCameraMoverParametersReset;
    private bool _isCameraRotatorParametersReset;

    public Quaternion PreviousBallRotation => _previousBallRotation;
    public Vector3 PreviousBallPosition => _previousBallPosition;
    public Quaternion BallRotation => _ball.transform.rotation;
    public Vector3 BallPosition => _ball.transform.position;

    private void Awake()
    {
        _cameraMover.ParametersReset += OnCameraMoverParametersReset;
        _cameraRotator.ParametersReset += OnCameraRotatorParametersReset;
    }

    private void Start()
    {
        SetPreviousBallTransform();
    }

    private void OnDestroy()
    {
        if (_cameraMover != null)
            _cameraMover.ParametersReset -= OnCameraMoverParametersReset;
        if (_cameraRotator != null)
            _cameraRotator.ParametersReset -= OnCameraRotatorParametersReset;
    }

    private void OnCameraRotatorParametersReset()
    {
        _isCameraRotatorParametersReset = true;
        if (_cameraMover.HasAdders() == false || _isCameraMoverParametersReset)
            SetPreviousBallTransform();
    }

    private void OnCameraMoverParametersReset()
    {
        _isCameraMoverParametersReset = true;
        if (_cameraRotator.HasAdders() == false || _isCameraRotatorParametersReset)
            SetPreviousBallTransform();
    }

    private void SetPreviousBallTransform()
    {
        _previousBallRotation = _ball.transform.rotation;
        _previousBallPosition = _ball.transform.position;
        _isCameraMoverParametersReset = false;
        _isCameraRotatorParametersReset = false;
    }
}
