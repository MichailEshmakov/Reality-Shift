using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CameraMover : Singleton<CameraMover>
{
    private Vector3 _startOffset;
    private Quaternion _startRotation;
    private Vector3 _resultAdditivePosition;
    private Quaternion _resultAdditiveRotation;
    private Vector3 _previousCameraPosition;
    private Quaternion _previousCameraRotation;
    private Quaternion _previousBallRotation;
    private Vector3 _previousBallPosition;
    private bool _canMove;
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

    protected override void Awake()
    {
        SetStartParameters();
        base.Awake();
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        BallPlacer.BallPlaced += OnBallPlaced;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        BallPlacer.BallPlaced -= OnBallPlaced;
    }

    private void LateUpdate()
    {
        if (_canMove)
        {
            MainCamera.Instance.transform.position += _resultAdditivePosition;
            MainCamera.Instance.transform.rotation = _resultAdditiveRotation * MainCamera.Instance.transform.rotation;
        }

        ResetParameters();
    }

    public void AddPosition(Vector3 additivePosition)
    {
        if (_canMove)
            _resultAdditivePosition += additivePosition;
    }

    public void AddRotation(Quaternion additiveRotation)
    {
        if (_canMove)
            _resultAdditiveRotation = additiveRotation * _resultAdditiveRotation;
    }

    public void SubscribeCameraMovementEffect(CameraMovingEffect effect)
    {
        effect.Disabled += OnCameraMovementEffectDisabled;
    }

    private void SetStartParameters()
    {
        MainCamera.DoWhenAwaked(() =>
        {
            Ball.DoWhenAwaked(() =>
            {
                ResetParameters();
                _startRotation = MainCamera.Instance.transform.rotation;
                _startOffset = MainCamera.Instance.transform.position - Ball.Instance.transform.position;
                _isStartParametersSet = true;
                StartParametersSet?.Invoke();
            });
        });
    }

    private void OnBallPlaced()
    {
        ResetCameraPosition();
        ResetParameters();
        _canMove = true;
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        _canMove = false;
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

        if (MainCamera.Instance != null)
        {
            _previousCameraPosition = MainCamera.Instance.transform.position;
            _previousCameraRotation = MainCamera.Instance.transform.rotation;
        }

        if (Ball.Instance != null)
        {
            _previousBallRotation = Ball.Instance.transform.rotation;
            _previousBallPosition = Ball.Instance.transform.position;
        }
    }

    private void ResetCameraPosition()
    {
        if (MainCamera.Instance != null)
        {
            MainCamera.Instance.transform.rotation = _startRotation;
            if (Ball.Instance != null)
                MainCamera.Instance.transform.position = Ball.Instance.transform.position + _startOffset;
        }
    }
}
