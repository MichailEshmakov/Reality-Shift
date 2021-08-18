using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomEffect : Effect
{
    [SerializeField] private Camera _mainCamera;
    [Header("Perspective")]
    [SerializeField] private float _maxFieldOfView;
    [SerializeField] private float _minFieldOfView;
    [SerializeField] private float _fieldOfViewSpeed;
    [Header("Orthographic")]
    [SerializeField] private float _maxOrthographicSize;
    [SerializeField] private float _minOrthographicSize;
    [SerializeField] private float _orthographicSizeSpeed;
    
    private bool _isInreasing;
    private float _startFieldOfView;
    private float _startOrthographicSize;

    private void Awake()
    {
        _startOrthographicSize = _mainCamera.orthographicSize;
        _startFieldOfView = _mainCamera.fieldOfView;
    }

    private void LateUpdate()
    {
        if (_mainCamera.orthographic)
            _mainCamera.orthographicSize = Zoom(_maxOrthographicSize, _minOrthographicSize, _mainCamera.orthographicSize, _orthographicSizeSpeed);
        else
            _mainCamera.fieldOfView = Zoom(_maxFieldOfView, _minFieldOfView, _mainCamera.fieldOfView, _fieldOfViewSpeed);
    }

    protected override void OnDisable()
    {
        if (_mainCamera != null)
        {
            _mainCamera.fieldOfView = _startFieldOfView;
            _mainCamera.orthographicSize = _startOrthographicSize;
        }

        base.OnDisable();
    }

    private float Zoom(float maxValue, float minValue, float value, float speed)
    {
        value = Mathf.MoveTowards(value, _isInreasing ? maxValue : minValue, speed * Time.deltaTime);

        if (value >= maxValue)
            _isInreasing = false;
        else if (value <= minValue)
            _isInreasing = true;

        return value;
    }
}
