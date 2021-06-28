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
        CameraMover.SubscribeCameraMovementEffect(this);
        CameraMover.CameraMovementEffectDisabled += OnCameraMovementEffectDisabled;
        Reset();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        CameraMover.CameraMovementEffectDisabled -= OnCameraMovementEffectDisabled;
    }

    private void OnCameraMovementEffectDisabled()
    {
        Reset();
    }

    protected virtual void Reset() { }

}
