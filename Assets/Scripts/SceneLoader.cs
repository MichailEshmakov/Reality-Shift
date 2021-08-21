using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int _defaultSceneIndex;
    [SerializeField] private LevelSaveSystem _levelSaveSystem;

    private Finish _finish;
    private bool _isLevelFinished = false;

    private void Awake()
    {
        SetFinish();
        _levelSaveSystem.ProgressSaved += OnProgressSaved;
    }

    private void OnDestroy()
    {
        _finish.LevelFinished -= OnLevelFinished;
        _levelSaveSystem.ProgressSaved -= OnProgressSaved;
    }

    private void OnProgressSaved()
    {
        int nextSceneIndex = _levelSaveSystem.CurrentLevelGroupProgress.SceneIndex;
        if (_isLevelFinished)
        {
            if (nextSceneIndex > 0 && nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(nextSceneIndex);
            else
                SceneManager.LoadScene(_defaultSceneIndex);
        }
    }

    private void OnLevelFinished()
    {
        _isLevelFinished = true;
    }

    private void SetFinish()
    {
        _finish = FindObjectOfType<Finish>();
        if (_finish == null)
            Debug.LogError($"Finish is not found by {gameObject.name}");
        else
            _finish.LevelFinished += OnLevelFinished;
    }
}
