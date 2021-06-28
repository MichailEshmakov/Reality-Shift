using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthographicCamraEffect : Effect
{
    [SerializeField] private Camera _camera;

    protected override void OnEnable()
    {
        base.OnEnable();
        _camera.orthographic = true;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _camera.orthographic = false;
    }
}
