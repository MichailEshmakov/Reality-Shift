using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdoptionEffect : CameraMovingEffect
{
    [SerializeField] private Transform _player;

    private Quaternion _onEnablePlayerRotation;

    private void Update()
    {
        CameraMover.AddPosition((_player.rotation * Quaternion.Inverse(_onEnablePlayerRotation) * CameraMover.StartOffset) 
            - (CameraMover.PreviousPlayerRotation * Quaternion.Inverse(_onEnablePlayerRotation) * CameraMover.StartOffset));
        CameraMover.AddRotation(_player.rotation * Quaternion.Inverse(CameraMover.PreviousPlayerRotation));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _onEnablePlayerRotation = _player.rotation;
    }
}
