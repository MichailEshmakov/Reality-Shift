using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraTransformingEffect : Effect
{
    [SerializeField] private CameraMover _cameraMover;
    [SerializeField] private CameraRotator _cameraRotator;

    protected CameraMover CameraMover => _cameraMover;
    protected CameraRotator CameraRotator => _cameraRotator;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_cameraMover != null)
        {
            _cameraMover.AdderDisabled += OnAdderDisabled;
            _cameraMover.AdderEnabled += OnAdderEnabled;
        }

        if (_cameraRotator != null)
        {
            _cameraRotator.AdderDisabled += OnAdderDisabled;
            _cameraRotator.AdderEnabled += OnAdderEnabled;
        }

        ResetParameters();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_cameraMover != null)
        {
            _cameraMover.AdderDisabled -= OnAdderDisabled;
            _cameraMover.AdderEnabled -= OnAdderEnabled;
        }

        if (_cameraRotator != null)
        {
            _cameraRotator.AdderDisabled -= OnAdderDisabled;
            _cameraRotator.AdderEnabled -= OnAdderEnabled;
        }
    }

    private void OnAdderDisabled()
    {
        ResetParameters();
    }

    private void OnAdderEnabled()
    {
        ResetParameters();
    }

    protected abstract void ResetParameters();
}
