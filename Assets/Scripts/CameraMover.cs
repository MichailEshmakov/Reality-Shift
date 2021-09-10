using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : CameraTransformer<ICameraMovingAdder>
{
    [SerializeField] private BallTransformObserver _ballTransformObserver;
    [SerializeField] private CameraDefaultFollower _defaultFollower;

    private Vector3 _startOffset;
    private Vector3 _resultAdditivePosition;

    public Vector3 StartOffset => _startOffset;

    protected override void Awake()
    {
        base.Awake();
        if (_defaultFollower is ICameraMovingAdder adder)
            AddAdder(adder);
    }

    private void Start()
    {
        _startOffset = MainCamera.transform.position - _ballTransformObserver.BallPosition;
        ResetFrameParameters();
    }

    private void ResetCameraPosition()
    {
        if (MainCamera != null && _ballTransformObserver != null)
            MainCamera.transform.position = _ballTransformObserver.BallPosition + _startOffset;
    }

    private void TryApplyAdditivePosition()
    {
        if (IsAllAddersUpdated())
        {
            MainCamera.transform.position += _resultAdditivePosition;
            ResetFrameParameters();
        }
    }

    protected override void ResetTransform()
    {
        ResetCameraPosition();
    }

    protected override void ResetFrameParameters()
    {
        _resultAdditivePosition = Vector3.zero;
        base.ResetFrameParameters();
    }

    public void AddPosition(Vector3 additivePosition, ICameraMovingAdder movingAdder)
    {
        if (IsAddersContains(movingAdder))
        {
            _resultAdditivePosition += additivePosition;
            UpdateAdder(movingAdder);
            TryApplyAdditivePosition();
        }
    }
}
