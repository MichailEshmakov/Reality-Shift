using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : CameraTransformer<ICameraRotatingAdder>
{
    private Quaternion _resultAdditiveRotation;
    private Quaternion _startRotation;

    private void Start()
    {
        _startRotation = MainCamera.transform.rotation;
        ResetFrameParameters();
    }

    private void TryApplyAdditiveRotation()
    {
        if (IsAllAddersUpdated())
        {
            MainCamera.transform.rotation = _resultAdditiveRotation * MainCamera.transform.rotation;
            ResetFrameParameters();
        }
    }

    private void ResetCameraRotation()
    {
        if (MainCamera != null)
            MainCamera.transform.rotation = _startRotation;
    }

    protected override void ResetTransform()
    {
        ResetCameraRotation();
    }

    protected override void ResetFrameParameters()
    {
        _resultAdditiveRotation = Quaternion.Euler(Vector3.zero);
        base.ResetFrameParameters();
    }

    public void AddRotation(Quaternion additiveRotation, ICameraRotatingAdder rotatingAdder)
    {
        if (IsAddersContains(rotatingAdder))
        {
            _resultAdditiveRotation = additiveRotation * _resultAdditiveRotation;
            UpdateAdder(rotatingAdder);
            TryApplyAdditiveRotation();
        }
    }
}
