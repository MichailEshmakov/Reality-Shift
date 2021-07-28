using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomEffect : Effect
{
    [Header("Perspective")]
    [SerializeField] private float _maxFieldOfView;
    [SerializeField] private float _minFieldOfView;
    [SerializeField] private float _fieldOfViewSpeed;
    [Header("Orthographic")]
    [SerializeField] private float _maxOrthographicSize;
    [SerializeField] private float _minOrthographicSize;
    [SerializeField] private float _orthographicSizeSpeed;
    
    private Camera _camera;
    private bool _isInreasing;
    private float _startFieldOfView;
    private float _startOrthographicSize;

    private void Awake()
    {
        MainCamera.DoWhenAwaked(() =>
        {
            _camera = MainCamera.Instance.GetComponent<Camera>();
            _startOrthographicSize = _camera.orthographicSize;
            _startFieldOfView = _camera.fieldOfView;
        });
    }

    private void LateUpdate()
    {
        if (_camera.orthographic)
            _camera.orthographicSize = Zoom(_maxOrthographicSize, _minOrthographicSize, _camera.orthographicSize, _orthographicSizeSpeed);
        else
            _camera.fieldOfView = Zoom(_maxFieldOfView, _minFieldOfView, _camera.fieldOfView, _fieldOfViewSpeed);
    }

    protected override void OnDisable()
    {
        _camera.fieldOfView = _startFieldOfView;
        _camera.orthographicSize = _startOrthographicSize;
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
