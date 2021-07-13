using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShimmerEffect : Effect
{
    [SerializeField] private List<Color> _colors;
    [SerializeField] private float _colorSpeed;
    [SerializeField] private float _maxIntensivity;
    [SerializeField] private float _minIntensivity;
    [SerializeField] private float _intensivitySpeed;

    private Light _light;
    private bool _isIncreasingIntensivity;
    private int _neededColorIndex = 0;
    private float _defaultIntensivity;
    private Color _defaultColor;

    private void Awake()
    {
        FindLight();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_light != null)
        {
            _defaultIntensivity = _light.intensity;
            _defaultColor = _light.color;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ResetLightProrerties();
    }

    private void Update()
    {
        ChangeIntensivity();
        ChangeColor();
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        ResetLightProrerties();
        _light = null;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        FindLight();
    }

    private void ChangeIntensivity()
    {
        if (_light != null)
        {
            _light.intensity = Mathf.MoveTowards(_light.intensity, _isIncreasingIntensivity ? _maxIntensivity : _minIntensivity, _intensivitySpeed * Time.deltaTime);

            if (_isIncreasingIntensivity && _light.intensity >= _maxIntensivity)
                _isIncreasingIntensivity = false;
            else if (_isIncreasingIntensivity == false && _light.intensity <= _minIntensivity)
                _isIncreasingIntensivity = true;
        }
    }

    private void ChangeColor()
    {
        if (_light != null)
        {
            _light.color = Color.Lerp(_light.color, _colors[_neededColorIndex], _intensivitySpeed * Time.deltaTime);
            if (_light.color == _colors[_neededColorIndex])
                SetNextColor();
        }
    }

    private void SetNextColor()
    {
        if (_neededColorIndex < _colors.Count - 1)
            _neededColorIndex++;
        else
            _neededColorIndex = 0;
    }

    private void FindLight()
    {
        MainLight mainLight = FindObjectOfType<MainLight>();
        if (mainLight != null)
            _light = mainLight.GetComponent<Light>();
        else
            _light = FindObjectOfType<Light>();
    }

    private void ResetLightProrerties()
    {
        if (_light != null)
        {
            _light.color = _defaultColor;
            _light.intensity = _defaultIntensivity;
        }
    }
}
