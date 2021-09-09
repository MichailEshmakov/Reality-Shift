using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraMovingEffect : Effect
{
    [SerializeField] private CameraMover _cameraMover;

    protected CameraMover CameraMover => _cameraMover;

    protected override void OnEnable()
    {
        base.OnEnable();
        _cameraMover.AdderDisabled += OnCameraMovingAdderDisabled;
        _cameraMover.AdderEnabled += OnCameraMovingAdderEnabled;

        ResetParameters();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_cameraMover != null)
        {
            _cameraMover.AdderDisabled -= OnCameraMovingAdderDisabled;
            _cameraMover.AdderEnabled += OnCameraMovingAdderEnabled;
        }
    }

    private void OnCameraMovingAdderDisabled()
    {
        ResetParameters();
    }

    private void OnCameraMovingAdderEnabled()
    {
        ResetParameters();
    }

    protected virtual void ResetParameters() { }
}
