using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraMovingEffect : Effect
{    
    protected override void OnEnable()
    {
        base.OnEnable();
        CameraMover.Instance.SubscribeCameraMovementEffect(this);
        CameraMover.Instance.CameraMovementEffectDisabled += OnCameraMovementEffectDisabled;
        StartPlayerPlacer.PlayerPlaced += OnPlayerPlaced;
        ResetParameters();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        CameraMover.Instance.CameraMovementEffectDisabled -= OnCameraMovementEffectDisabled;
        StartPlayerPlacer.PlayerPlaced -= OnPlayerPlaced;
    }

    private void OnPlayerPlaced()
    {
        ResetParameters();
    }

    private void OnCameraMovementEffectDisabled()
    {
        ResetParameters();
    }

    protected virtual void ResetParameters() { }

}
