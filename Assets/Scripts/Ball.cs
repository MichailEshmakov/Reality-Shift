using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    //TODO: Разделить управление и смерть
    [SerializeField] private float _movingForce;
    [SerializeField] private InverseInputEffect _inverseInputEffect;
    [SerializeField] private float _platformCoefficient;
    [SerializeField] private float _breakingDeathDelay;
    [SerializeField] private BallPart[] _breakingParts;
    [SerializeField] private GameObject _model;
    [SerializeField] private BallPlacer _placer;
    [SerializeField] private float _breakingForce;

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

        _placer.BallPlaced += OnPlaced;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out BallBreaker ballBreaker))
        {
            Break();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out LevelBorder levelBorder))
            Die();
    }

    private void OnPlaced()
    {
        _model.SetActive(true);
        _rigidbody.useGravity = true;
        if (gameObject.activeSelf)
            _input.Enable();
    }

    private void Break()
    {
        _model.SetActive(false);
        _rigidbody.useGravity = false;
        _input.Disable();
        ResetVelocity();
        foreach (BallPart part in _breakingParts)
        {
            BallPart newPart = Instantiate(part, transform);
            newPart.TakeForce(_breakingForce);
            Destroy(newPart.gameObject, _breakingDeathDelay);//TODO:Переделать в пул
        }

        StartCoroutine(DieWithDalay());
    }

    private IEnumerator DieWithDalay()
    {
        yield return new WaitForSeconds(_breakingDeathDelay);
        Die();
    }

    private void Die()
    {
        Died?.Invoke();
        ResetVelocity();
    }

    private void ResetVelocity()
    {
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
