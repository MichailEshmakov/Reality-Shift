using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdoptionEffect : CameraMovingEffect, ICameraRotatingAdder, ICameraMovingAdder
{
    [SerializeField] private Transform _ball;

    private Quaternion _resetedBallRotation;

    private void LateUpdate()
    {
        CameraMover.AddPosition((_ball.rotation * Quaternion.Inverse(_resetedBallRotation) * CameraMover.StartOffset) 
            - (CameraMover.PreviousBallRotation * Quaternion.Inverse(_resetedBallRotation) * CameraMover.StartOffset), this);
        CameraMover.AddRotation(_ball.rotation * Quaternion.Inverse(CameraMover.PreviousBallRotation), this);
    }

    protected override void ResetParameters()
    {
        _resetedBallRotation = _ball.rotation;
    }
}
