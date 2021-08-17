using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdoptionEffect : CameraMovingEffect
{
    [SerializeField] private Transform _ball;

    private Quaternion _onEnableBallRotation;

    private void Update()
    {
        CameraMover.Instance.AddPosition((_ball.rotation * Quaternion.Inverse(_onEnableBallRotation) * CameraMover.Instance.StartOffset) 
            - (CameraMover.Instance.PreviousBallRotation * Quaternion.Inverse(_onEnableBallRotation) * CameraMover.Instance.StartOffset));
        CameraMover.Instance.AddRotation(_ball.rotation * Quaternion.Inverse(CameraMover.Instance.PreviousBallRotation));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _onEnableBallRotation = _ball.rotation;
    }
}
