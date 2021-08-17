using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDefaultFollower : Singleton<CameraDefaultFollower>
{
    private void Update()
    {
        CameraMover.Instance.AddPosition(Ball.Instance.transform.position - CameraMover.Instance.PreviousBallPosition);
    }
}
