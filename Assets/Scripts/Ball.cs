using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float _movingForce;
    [SerializeField] private InverseInputEffect _inverseInputEffect;
    [SerializeField] private float _platformCoefficient;

    private PlayerInput _input;
    private Rigidbody _rigidbody;
    private int _inversingCoefficient = 1;

    public event UnityAction Died;

    private void Awake()
    {
        if (_inverseInputEffect != null)
        {
            _inverseInputEffect.Disabled += OnInverseInputEffectDisabled;
            _inverseInputEffect.Enabled += OnInverseInputEffectEnabled;
            if (_inverseInputEffect.enabled)
                _inversingCoefficient = -1;
        }

        _input = new PlayerInput();
        _rigidbody = GetComponent<Rigidbody>();
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
        if (_inverseInputEffect != null)
        {
            _inverseInputEffect.Disabled -= OnInverseInputEffectDisabled;
            _inverseInputEffect.Enabled -= OnInverseInputEffectEnabled;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out LevelBorder levelBorder))
            Die();
    }

    private void Die()
    {
        Died?.Invoke();
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
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
