using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdoptionEffect : CameraMovingEffect
{
    [SerializeField] private Transform _player;

    private Quaternion _onEnablePlayerRotation;

    private void Update()
    {
        CameraMover.Instance.AddPosition((_player.rotation * Quaternion.Inverse(_onEnablePlayerRotation) * CameraMover.Instance.StartOffset) 
            - (CameraMover.Instance.PreviousPlayerRotation * Quaternion.Inverse(_onEnablePlayerRotation) * CameraMover.Instance.StartOffset));
        CameraMover.Instance.AddRotation(_player.rotation * Quaternion.Inverse(CameraMover.Instance.PreviousPlayerRotation));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _onEnablePlayerRotation = _player.rotation;
    }
}
