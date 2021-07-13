using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDefaultFollower : Singleton<CameraDefaultFollower>
{
    private void Update()
    {
        CameraMover.Instance.AddPosition(Player.Instance.transform.position - CameraMover.Instance.PreviousPlayerPosition);
    }
}
