using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Player : Singleton<Player>
{
    [SerializeField] private float _movingForce;
    [SerializeField] private int _questions;
    [SerializeField] private InverseInputEffect _inverseInputEffect;
    [SerializeField] private float _platformCoefficient;

    private PlayerInput _input;
    private Rigidbody _rigidbody;
    private int _inversingCoefficient = 1;
    private int _questionsOnThisLevel;

    public event UnityAction<int> QuestionsChanged;
    public event UnityAction Died;

    protected override void Awake()
    {
        if (_inverseInputEffect != null)
        {
            _inverseInputEffect.Disabled += OnInverseInputEffectDisabled;
            _inverseInputEffect.Enabled += OnInverseInputEffectEnabled;
        }

        SceneManager.sceneUnloaded += OnSceneUnloaded;
        base.Awake();
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

    private void Start()
    {
        QuestionsChanged?.Invoke(_questions);
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

    private void OnSceneUnloaded(Scene arg0)
    {
        _questions += _questionsOnThisLevel;
        _questionsOnThisLevel = 0;
    }

    private void Die()
    {
        Died?.Invoke();
        _questionsOnThisLevel = 0;
        QuestionsChanged?.Invoke(_questions);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    public void AddQuestion()
    {
        _questionsOnThisLevel++;
        QuestionsChanged?.Invoke(_questions + _questionsOnThisLevel);
    }

    public bool TryPayQuestions(int price)
    {
        if (_questions >= price)
        {
            _questions -= price;
            QuestionsChanged?.Invoke(_questions + _questionsOnThisLevel);
            return true;
        }

        return false;
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
