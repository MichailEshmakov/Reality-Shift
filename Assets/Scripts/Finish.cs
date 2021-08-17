using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] private float _endDelay;

    bool _isBallInside;

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
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentLevelIndex != SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene(currentLevelIndex + 1);
        else
            SceneManager.LoadScene(0);
    }
}
