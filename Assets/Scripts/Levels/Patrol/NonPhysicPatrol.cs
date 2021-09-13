using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPhysicPatrol : Patrol
{
    private void Update()
    {
        if (TargetPoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPoint.transform.position, Speed * Time.deltaTime);
            if (transform.position == TargetPoint.transform.position)
                SetNextTargetPoint();
        }
    }
}
