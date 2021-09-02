using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraMovingEffect : Effect
{
    [SerializeField] private CameraMover _cameraMover;

    protected CameraMover CameraMover => _cameraMover;

    protected override void OnEnable()
    {
        base.OnEnable();
        _cameraMover.SubscribeCameraMovementEffect(this);
        _cameraMover.CameraMovementEffectDisabled += OnCameraMovementEffectDisabled;

        ResetParameters();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_cameraMover != null)
            _cameraMover.CameraMovementEffectDisabled -= OnCameraMovementEffectDisabled;
    }

    private void OnCameraMovementEffectDisabled()
    {
        ResetParameters();
    }

    protected virtual void ResetParameters() { }

}
