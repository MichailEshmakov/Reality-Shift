using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdoptionEffect : Effect
{
    [SerializeField] CameraMover _cameraMover;
    [SerializeField] Transform _player;

    private void Update()
    {
        _cameraMover.AddPosition((_player.rotation * _cameraMover.StartOffset) -_cameraMover.StartOffset);
        _cameraMover.AddRotation(_player.rotation * _cameraMover.StartRotation * Quaternion.Inverse(_cameraMover.PreviousCameraRotation));
    }

    protected override void OnDisable()
    {
        _cameraMover.AddRotation(_cameraMover.StartRotation * Quaternion.Inverse(_cameraMover.PreviousCameraRotation));
        base.OnDisable();
    }
}
