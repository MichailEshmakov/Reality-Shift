using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] private float _endDelay;

    private bool _isBallInside;

    public event UnityAction LevelFinished;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Ball ball))
        {
            _isBallInside = true;
            StartCoroutine(WaitFinish());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Ball ball))
        {
            _isBallInside = false;
        }
    }

    private IEnumerator WaitFinish()
    {
        yield return new WaitForSeconds(_endDelay);
        if (_isBallInside)
            FinishLevel();
    }

    private void FinishLevel()
    {
        LevelFinished?.Invoke();
    }
}
