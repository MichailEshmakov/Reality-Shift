using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdoptionEffect : CameraMovingEffect
{
    [SerializeField] private Transform _ball;

    private Quaternion _onEnableBallRotation;

    private void Update()
    {
        CameraMover.AddPosition((_ball.rotation * Quaternion.Inverse(_onEnableBallRotation) * CameraMover.StartOffset) 
            - (CameraMover.PreviousBallRotation * Quaternion.Inverse(_onEnableBallRotation) * CameraMover.StartOffset));
        CameraMover.AddRotation(_ball.rotation * Quaternion.Inverse(CameraMover.PreviousBallRotation));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _onEnableBallRotation = _ball.rotation;
    }
}
