using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ball))]
[RequireComponent(typeof(Rigidbody))]
public class BallMover : MonoBehaviour
{
    [SerializeField] private float _movingForce;
    [SerializeField] private InverseInputEffect _inverseInputEffect;
    [SerializeField] private float _platformCoefficient;
    [SerializeField] private BallPlacer _placer;

    private Ball _ball;
    private PlayerInput _input;
    private Rigidbody _rigidbody;
    private int _inversingCoefficient = 1;

    private void Awake()
    {
        _ball = GetComponent<Ball>();
        _ball.Broke += OnBroke;
        if (_inverseInputEffect != null)
        {
            _inverseInputEffect.Disabled += OnInverseInputEffectDisabled;
            _inverseInputEffect.Enabled += OnInverseInputEffectEnabled;
            if (_inverseInputEffect.enabled)
                _inversingCoefficient = -1;
        }

        _input = new PlayerInput();
        _rigidbody = GetComponent<Rigidbody>();
        _placer.BallPlaced += OnPlaced;
        if (Application.platform == RuntimePlatform.WindowsEditor)
            _platformCoefficient = 1;
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        if (_input != null)
            _input.Disable();
    }

    private void FixedUpdate()
    {
        if (Application.platform != RuntimePlatform.WindowsEditor || _input.Player.AllowMove.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            Vector2 movingInput = _input.Player.Move.ReadValue<Vector2>() * _inversingCoefficient;
            _rigidbody.AddForce(new Vector3(movingInput.x, 0, movingInput.y) * _movingForce * _platformCoefficient * Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        _ball.Broke -= OnBroke;
        if (_placer != null)
            _placer.BallPlaced -= OnPlaced;

        if (_inverseInputEffect != null)
        {
            _inverseInputEffect.Disabled -= OnInverseInputEffectDisabled;
            _inverseInputEffect.Enabled -= OnInverseInputEffectEnabled;
        }
    }

    private void OnPlaced()
    {
        enabled = true;
    }

    private void OnBroke()
    {
        enabled = false;
    }

    private void OnInverseInputEffectDisabled(Effect effect)
    {
        _inversingCoefficient = 1;
    }

    private void OnInverseInputEffectEnabled(Effect effect)
    {
        _inversingCoefficient = -1;
    }
}
