using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private FireCarrier _master;

    private void Update()
    {
        if (_master != null)
            transform.position = _master.transform.position;
    }

    public void Init(FireCarrier master)
    {
        _master = master;
    }
}
