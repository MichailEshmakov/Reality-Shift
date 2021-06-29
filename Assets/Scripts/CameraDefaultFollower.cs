using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDefaultFollower : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private CameraMover _cameraMover;

    private void Update()
    {
        _cameraMover.AddPosition(_player.position - _cameraMover.PreviousPlayerPosition);
    }
}
