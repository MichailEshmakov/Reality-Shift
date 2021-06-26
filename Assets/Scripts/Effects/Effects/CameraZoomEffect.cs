using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomEffect : Effect
{
    [SerializeField] private float _maxFieldOfView;
    [SerializeField] private float _minFieldOfView;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private Camera _camera;

    private bool _isInreasing;
    private float _startFieldOfView;

    private void Start()
    {
        _startFieldOfView = _camera.fieldOfView;
    }

    private void LateUpdate()
    {
        _camera.fieldOfView = Mathf.MoveTowards(_camera.fieldOfView, _isInreasing ? _maxFieldOfView : _minFieldOfView, _zoomSpeed * Time.deltaTime);

        if (_camera.fieldOfView >= _maxFieldOfView)
            _isInreasing = false;
        else if (_camera.fieldOfView <= _minFieldOfView)
            _isInreasing = true;
    }

    protected override void OnDisable()
    {
        _camera.fieldOfView = _startFieldOfView;
        base.OnDisable();
    }
}
