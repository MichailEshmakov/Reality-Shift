using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float _breakingDeathDelay;
    [SerializeField] private BallPart[] _breakingParts;
    [SerializeField] private GameObject _model;
    [SerializeField] private float _breakingForce;

    private Rigidbody _rigidbody;

    public event UnityAction Died;
    public event UnityAction Broke;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
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

    private void Break()
    {
        _model.SetActive(false);
        _rigidbody.useGravity = false;
        ResetVelocity();
        foreach (BallPart part in _breakingParts)
        {
            BallPart newPart = Instantiate(part, transform);
            newPart.TakeForce(_breakingForce);
            Destroy(newPart.gameObject, _breakingDeathDelay);//TODO:Переделать в пул
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
