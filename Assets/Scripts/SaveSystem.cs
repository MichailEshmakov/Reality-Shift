using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private QuestionScore _questionScore;
    [SerializeField] private EffectKeeper _effectKeeper;
    [SerializeField] private LevelGroupKeeper _levelGroupKeeper;

    private class LevelGroupProgress
    {
        public int NextSceneIndex;
        public int Questions;
        public int[] EffectIndexes;

        public LevelGroupProgress(int sceneIndex, int questions, int[] effectIndexes)
        {
            NextSceneIndex = sceneIndex;
            Questions = questions;
            EffectIndexes = effectIndexes;
        }
    }

    private LevelGroupProgress _currentLevelGroupProgress;
    private bool _isProgressDownloaded = false;

    public event UnityAction<int> ProgressSaved;
    public event UnityAction ProgressDownloaded;

    public int SavedQuestions => _currentLevelGroupProgress.Questions;
    public bool IsProgressDownloaded => _isProgressDownloaded;
    public int[] SavedEffectIndexes
    {
        get
        {
            int[] savedEffectIndexes = new int[_currentLevelGroupProgress.EffectIndexes.Length];
            Array.Copy(_currentLevelGroupProgress.EffectIndexes, savedEffectIndexes, _currentLevelGroupProgress.EffectIndexes.Length);
            return savedEffectIndexes;
        }
    }

    private void Awake()
    {
        _questionScore.LevelQuestionsRecorded += OnLevelQuestionsRecorded;
        if (_levelGroupKeeper.LevelGroup.GetCurrentLevelIndex() > 0)
            DownloadProgress();
    }

    private void OnDestroy()
    {
        _questionScore.LevelQuestionsRecorded -= OnLevelQuestionsRecorded;
    }

    private void OnLevelQuestionsRecorded(int questions)
    {
        SaveProgress(questions);
    }

    private void SaveProgress(int questions)
    {
        int nextSceneIndex = _levelGroupKeeper.LevelGroup.GetNextSceneIndex();
        _currentLevelGroupProgress = new LevelGroupProgress(nextSceneIndex, questions, _effectKeeper.GetEnabledEffectsIndexes().ToArray());
        PlayerPrefs.SetString(_levelGroupKeeper.LevelGroup.name, JsonUtility.ToJson(_currentLevelGroupProgress));
        ProgressSaved?.Invoke(nextSceneIndex);
    }

    private void DownloadProgress()
    {
        string jsonProgressData = PlayerPrefs.GetString(_levelGroupKeeper.LevelGroup.name);
        _currentLevelGroupProgress = JsonUtility.FromJson<LevelGroupProgress>(jsonProgressData);
        _isProgressDownloaded = true;
        ProgressDownloaded?.Invoke();
    }
}
