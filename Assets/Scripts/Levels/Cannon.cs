using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : ObjectPool
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _shootingDelay;
    [SerializeField] private Bullet _bulletTemplate;
    [SerializeField] private Transform _raycastOrigin;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Transform _muzzleDirectionPoint;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _shootingImpulse;
    [SerializeField] private float _shootingRadius;

    private Transform _target;
    private float _lastShootingTime;
    private Vector3 _startShootingDirection;
    private Quaternion _startRotation;

    private void Awake()
    {
        Initialize(_bulletTemplate.gameObject);
    }

    private void Start()
    {
        _lastShootingTime = Time.time;
        _startShootingDirection = (_muzzleDirectionPoint.position - _bulletSpawnPoint.position).normalized;
        _startRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        Ball ball = other.GetComponentInParent<Ball>();
        if (ball != null)
        {
            _target = ball.transform;
            StartCoroutine(RotateToTarget());
            StartCoroutine(TryShoot());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Ball>() != null)
            _target = null;
    }

    private IEnumerator RotateToTarget()
    {
        while (_target != null)
        {
            Quaternion neededRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.position - _target.position, transform.up));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, neededRotation, _rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator TryShoot()
    {
        while (_target != null)
        {
            if (CheckShootAbility(out Bullet bullet))
                Shoot(bullet);

            yield return new WaitForFixedUpdate();
        }
    }

    private bool CheckShootAbility(out Bullet bullet)
    {
        bullet = null;
        return Time.time - _lastShootingTime >= _shootingDelay
            && Physics.Raycast(_raycastOrigin.position, -transform.forward, out RaycastHit hit, _shootingRadius, _layerMask)
            && hit.collider.gameObject.GetComponentInParent<Ball>() != null
            && TryGetObject(out GameObject bulletGameObject)
            && bulletGameObject.TryGetComponent(out bullet);
    }

    private void Shoot(Bullet bullet)
    {
        if (bullet.Rigidbody != null)
        {
            bullet.transform.position = _bulletSpawnPoint.position;
            bullet.transform.rotation = _bulletSpawnPoint.rotation;
            bullet.gameObject.SetActive(true);
            bullet.Rigidbody.velocity = Vector3.zero;
            bullet.Rigidbody.angularVelocity = Vector3.zero;
            Vector3 shootingDirection = (transform.rotation * Quaternion.Inverse(_startRotation)) * _startShootingDirection;
            bullet.Rigidbody.AddForce(shootingDirection * _shootingImpulse, ForceMode.Impulse);
            _lastShootingTime = Time.time;
        }
    }
}
