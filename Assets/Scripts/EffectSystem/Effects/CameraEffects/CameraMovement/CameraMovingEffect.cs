using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraMovingEffect : Effect
{    
    protected override void OnEnable()
    {
        base.OnEnable();
        CameraMover.DoWhenAwaked(() =>
        {
            CameraMover.Instance.SubscribeCameraMovementEffect(this);
            CameraMover.Instance.CameraMovementEffectDisabled += OnCameraMovementEffectDisabled;
        });

        PlayerPlacer.PlayerPlaced += OnPlayerPlaced;
        ResetParameters();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (CameraMover.Instance != null)
            CameraMover.Instance.CameraMovementEffectDisabled -= OnCameraMovementEffectDisabled;

        PlayerPlacer.PlayerPlaced -= OnPlayerPlaced;
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
