using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShimmerEffect : Effect
{
    [SerializeField] private Light _light;
    [SerializeField] private List<Color> _colors;
    [SerializeField] private float _colorSpeed;
    [SerializeField] private float _maxIntensivity;
    [SerializeField] private float _minIntensivity;
    [SerializeField] private float _intensivitySpeed;

    private bool _isIncreasingIntensivity;
    private int _neededColorIndex = 0;

    private void Update()
    {
        ChangeIntensivity();
        ChangeColor();
    }

    private void ChangeIntensivity()
    {
        _light.intensity = Mathf.MoveTowards(_light.intensity, _isIncreasingIntensivity ? _maxIntensivity : _minIntensivity, _intensivitySpeed * Time.deltaTime);

        if (_isIncreasingIntensivity && _light.intensity >= _maxIntensivity)
            _isIncreasingIntensivity = false;
        else if (_isIncreasingIntensivity == false && _light.intensity <= _minIntensivity)
            _isIncreasingIntensivity = true;
    }

    private void ChangeColor()
    {
        _light.color = Color.Lerp(_light.color, _colors[_neededColorIndex], _intensivitySpeed * Time.deltaTime);
        if (_light.color == _colors[_neededColorIndex])
            SetNextColor();
    }

    private void SetNextColor()
    {
        if (_neededColorIndex < _colors.Count - 1)
            _neededColorIndex++;
        else
            _neededColorIndex = 0;
    }
}
