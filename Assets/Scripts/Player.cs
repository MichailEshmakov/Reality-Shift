using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _movingForce;

    private PlayerInput _input;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _input = new PlayerInput();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void FixedUpdate()
    {
        if (Application.platform != RuntimePlatform.WindowsEditor || _input.Player.AllowMove.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            Vector2 movingInput = _input.Player.Move.ReadValue<Vector2>();
            _rigidbody.AddForce(new Vector3(movingInput.x, 0, movingInput.y) * _movingForce * Time.deltaTime);
        }
    }
}
