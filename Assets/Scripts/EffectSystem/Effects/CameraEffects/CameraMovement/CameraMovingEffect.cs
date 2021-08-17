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

        BallPlacer.BallPlaced += OnBallPlaced;
        ResetParameters();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (CameraMover.Instance != null)
            CameraMover.Instance.CameraMovementEffectDisabled -= OnCameraMovementEffectDisabled;

        BallPlacer.BallPlaced -= OnBallPlaced;
    }

    private void OnBallPlaced()
    {
        ResetParameters();
    }

    private void OnCameraMovementEffectDisabled()
    {
        ResetParameters();
    }

    protected virtual void ResetParameters() { }

}
