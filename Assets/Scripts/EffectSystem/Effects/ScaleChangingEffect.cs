using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChangingEffect : Effect
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _maxScaleCoefficient;
    [SerializeField] private float _minScaleCoefficient;
    [SerializeField] private float _changingSpeed;

    private Vector3 _defaultScale;
    private bool _isScaleIncreasing;
    private float _currentScaleCoefficient;

    protected override void OnEnable()
    {
        base.OnEnable();
        _defaultScale = _player.localScale;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _player.localScale = _defaultScale;
    }

    private void Update()
    {
        _currentScaleCoefficient = Mathf.MoveTowards(_currentScaleCoefficient, _isScaleIncreasing ? _maxScaleCoefficient : _minScaleCoefficient, _changingSpeed * Time.deltaTime);
        _player.localScale = _defaultScale * _currentScaleCoefficient;

        if (_isScaleIncreasing && _currentScaleCoefficient >= _maxScaleCoefficient)
            _isScaleIncreasing = false;
        else if (_isScaleIncreasing == false && _currentScaleCoefficient <= _minScaleCoefficient)
            _isScaleIncreasing = true;
    }
}
