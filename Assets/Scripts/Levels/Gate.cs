using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private float _liftingHeight;
    [SerializeField] private float _liftingSpeed;
    [SerializeField] private KeyFallDetector _keyFallDetector;

    private float _startPositonY;

    private void Awake()
    {
        _keyFallDetector.KeyKeyFallen += OnKeyFallen;
        _startPositonY = transform.position.y;
    }

    private void OnDestroy()
    {
        _keyFallDetector.KeyKeyFallen -= OnKeyFallen;
    }

    private void OnKeyFallen()
    {
        StartCoroutine(Lift());
    }

    private IEnumerator Lift()
    {
        while (transform.position.y < _startPositonY + _liftingHeight)
        {
            transform.Translate(Vector3.up * _liftingSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
