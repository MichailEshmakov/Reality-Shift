using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float _breakingDeathDelay;
    [SerializeField] private GameObject _model;
    [SerializeField] private ShapeEffectHierarchy _shapeHierarchy;
    [SerializeField] private float _breakingForce;
    [SerializeField] private QuestionScore _questionScore;

    private Rigidbody _rigidbody;
    private FireCarrier _fireCarrier;

    public GameObject Model => _model;

    public event UnityAction Died;
    public event UnityAction Broke;
    public event UnityAction ShapeChanged;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _fireCarrier = GetComponent<FireCarrier>();
        _shapeHierarchy.NewShapeApplied += OnNewShapeApplied;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out BallBreaker ballBreaker))
        {
            if (_fireCarrier == null || _fireCarrier.IsBurning == false)
                Break();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Question question))
            _questionScore.AddQuestion();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out LevelBorder levelBorder))
            Die();
    }

    private void OnNewShapeApplied(GameObject newModel)
    {
        Destroy(_model);
        _model = newModel;
        ShapeChanged?.Invoke();
    }

    private void Break()
    {
        _model.SetActive(false);
        _rigidbody.useGravity = false;
        ResetVelocity();
        BallPart[] breakingParts = _shapeHierarchy.GetPartsTemplates();
        foreach (BallPart part in breakingParts)
        {
            BallPart newPart = Instantiate(part, transform);
            newPart.TakeForce(_breakingForce);
            Destroy(newPart.gameObject, _breakingDeathDelay);
        }

        Broke?.Invoke();
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
    }

    private void ResetVelocity()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}
