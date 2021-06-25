using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _movingForce;
    [SerializeField] int _questions;
    [SerializeField] InverseInputEffect _inverseInputEffect;

    private PlayerInput _input;
    private Rigidbody _rigidbody;
    private int _inversingCoefficient = 1;

    public event UnityAction<int> QuestionsChanged;

    private void Awake()
    {
        _input = new PlayerInput();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _input.Enable();
        if (_inverseInputEffect != null)
        {
            _inverseInputEffect.Disabled += OnInverseInputEffectDisabled;
            _inverseInputEffect.Enabled += OnInverseInputEffectEnabled;
        }

    }

    private void OnDisable()
    {
        _input.Disable();
        if (_inverseInputEffect != null)
        {
            _inverseInputEffect.Disabled -= OnInverseInputEffectDisabled;
            _inverseInputEffect.Enabled -= OnInverseInputEffectEnabled;
        }
    }

    private void FixedUpdate()
    {
        if (Application.platform != RuntimePlatform.WindowsEditor || _input.Player.AllowMove.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            Vector2 movingInput = _input.Player.Move.ReadValue<Vector2>() * _inversingCoefficient;
            _rigidbody.AddForce(new Vector3(movingInput.x, 0, movingInput.y) * _movingForce * Time.deltaTime);
        }
    }

    public void AddQuestion()
    {
        _questions++;
        QuestionsChanged?.Invoke(_questions);
    }

    public bool TryPayQuestions(int price)
    {
        if (_questions >= price)
        {
            _questions -= price;
            QuestionsChanged?.Invoke(_questions);
            return true;
        }

        return false;
    }

    private void OnInverseInputEffectDisabled()
    {
        _inversingCoefficient = 1;
    }

    private void OnInverseInputEffectEnabled()
    {
        _inversingCoefficient = -1;
    }
}
