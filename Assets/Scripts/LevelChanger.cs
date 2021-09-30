using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private int _defaultSceneIndex;
    [SerializeField] private LevelSaveSystem _levelSaveSystem;
    [SerializeField] private Finisher _finisher;

    private bool _isLevelFinished = false;

    private void Awake()
    {
        _finisher.LevelFinished += OnLevelFinished;
        _levelSaveSystem.ProgressSaved += OnProgressSaved;
    }

    private void OnDestroy()
    {
        if (_finisher != null)
            _finisher.LevelFinished -= OnLevelFinished;

        if (_levelSaveSystem != null)
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
}
