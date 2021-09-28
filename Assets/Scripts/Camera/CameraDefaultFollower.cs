using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDefaultFollower : MonoBehaviour, ICameraMovingAdder
{
    [SerializeField] private BallTransformObserver _BallTransformObserver;
    [SerializeField] private CameraMover _cameraMover;

    private void LateUpdate()
    {
        _cameraMover.AddPosition(_BallTransformObserver.BallPosition - _BallTransformObserver.PreviousBallPosition, this);
    }
}
