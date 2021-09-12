using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdoptionEffect : CameraTransformingEffect, ICameraRotatingAdder, ICameraMovingAdder
{
    [SerializeField] private BallTransformObserver _ballTransformObserver;

    private Quaternion _resetedBallRotation;

    private void LateUpdate()
    {
        CameraMover.AddPosition((_ballTransformObserver.BallRotation * Quaternion.Inverse(_resetedBallRotation) * CameraMover.StartOffset) 
            - (_ballTransformObserver.PreviousBallRotation * Quaternion.Inverse(_resetedBallRotation) * CameraMover.StartOffset), this);
        CameraRotator.AddRotation(_ballTransformObserver.BallRotation * Quaternion.Inverse(_ballTransformObserver.PreviousBallRotation), this);
    }

    protected override void ResetParameters()
    {
        _resetedBallRotation = _ballTransformObserver.BallRotation;
    }
}
