using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDefaultFollower : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private CameraMover _cameraMover;

    private void Update()
    {
        _cameraMover.AddPosition(_ball.transform.position - _cameraMover.PreviousBallPosition);
    }
}
