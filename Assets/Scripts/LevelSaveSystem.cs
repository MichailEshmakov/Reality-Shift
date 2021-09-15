using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelSaveSystem : SaveSystem
{
    [SerializeField] private QuestionScore _questionScore;
    [SerializeField] private EffectKeeper _effectKeeper;
    [SerializeField] private LevelGroupKeeper _levelGroupKeeper;

    private LevelGroupProgress _currentLevelGroupProgress;
    private bool _isProgressDownloaded = false;

    public event UnityAction ProgressDownloaded;
    public bool IsProgressDownloaded => _isProgressDownloaded;
    public LevelGroupProgress CurrentLevelGroupProgress => _currentLevelGroupProgress;

    private void Start()
    {
        _questionScore.LevelQuestionsRecorded += OnLevelQuestionsRecorded;
        if (_levelGroupKeeper.LevelGroup.GetCurrentLevelIndex() > 0)
        {
            _currentLevelGroupProgress = DownloadProgress(_levelGroupKeeper.LevelGroup.name);
            _isProgressDownloaded = true;
            ProgressDownloaded?.Invoke();
        }
    }

    private void OnDestroy()
    {
        _questionScore.LevelQuestionsRecorded -= OnLevelQuestionsRecorded;
    }

    private void OnLevelQuestionsRecorded(int questions)
    {
        int nextSceneIndex = _levelGroupKeeper.LevelGroup.GetNextSceneIndex();
        int[] enableEffectIndexes = _effectKeeper.GetEnabledEffectsIndexes().ToArray();
        string levelGroupName = _levelGroupKeeper.LevelGroup.name;
        _currentLevelGroupProgress = new LevelGroupProgress(nextSceneIndex, questions, enableEffectIndexes);
        SaveProgress(_currentLevelGroupProgress, levelGroupName);
    }
}
