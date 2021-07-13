using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] private float _endDelay;

    bool _isPlayerInside;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            _isPlayerInside = true;
            StartCoroutine(WaitFinish());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            _isPlayerInside = false;
        }
    }

    private IEnumerator WaitFinish()
    {
        yield return new WaitForSeconds(_endDelay);
        if (_isPlayerInside)
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
