using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelSaveSystem : SaveSystem
{
    [SerializeField] private QuestionScore _questionScore;
    [SerializeField] private EffectKeeper _effectKeeper;
    [SerializeField] private LevelGroupKeeper _levelGroupKeeper;
    [SerializeField] private ChooseEffectMenu _chooseEffectMenu;
 
    private LevelGroupProgress _currentLevelGroupProgress;
    private bool _isProgressSet = false;

    public bool IsProgressSet => _isProgressSet;
    public LevelGroupProgress CurrentLevelGroupProgress => _currentLevelGroupProgress;

    public event UnityAction ProgressSet;

    private void OnEnable()
    {
        _questionScore.LevelQuestionsRecorded += OnLevelQuestionsRecorded;
        _chooseEffectMenu.EffectChosen += OnEffectChosen;
    }

    private void OnDisable()
    {
        if (_questionScore != null)
            _questionScore.LevelQuestionsRecorded -= OnLevelQuestionsRecorded;

        if (_chooseEffectMenu != null)
            _chooseEffectMenu.EffectChosen -= OnEffectChosen;
    }

    private void Start()
    {
        if (_levelGroupKeeper.LevelGroup.GetCurrentLevelIndex() > 0)
            _currentLevelGroupProgress = DownloadProgress(_levelGroupKeeper.LevelGroup.name);
        else
            CreateLevelGroupProgress();

        _isProgressSet = true;
        ProgressSet?.Invoke();
    }

    private void OnEffectChosen(List<Effect> proposedEffects)
    {
        List<int> proposedEffectsIndexes = new List<int>(proposedEffects.Count);
        foreach (Effect effect in proposedEffects)
        {
            int index = _effectKeeper.GetIndex(effect);
            if (index >= 0)
                proposedEffectsIndexes.Add(index);
        }

        _currentLevelGroupProgress = new LevelGroupProgress(_currentLevelGroupProgress.SceneIndex, 
            _currentLevelGroupProgress.Questions, 
            _effectKeeper.GetEnabledEffectsIndexes().ToArray(),
            proposedEffectsIndexes.ToArray(),
            true);

        SaveProgress(_currentLevelGroupProgress, _levelGroupKeeper.LevelGroup.name);
    }

    private void OnLevelQuestionsRecorded(int questions)
    {
        int nextSceneIndex = _levelGroupKeeper.LevelGroup.GetNextSceneIndex();
        int[] enableEffectIndexes = _effectKeeper.GetEnabledEffectsIndexes().ToArray();
        string levelGroupName = _levelGroupKeeper.LevelGroup.name;
        _currentLevelGroupProgress = new LevelGroupProgress(nextSceneIndex, questions, enableEffectIndexes, _currentLevelGroupProgress.ProposedEffectIndexes);
        SaveProgress(_currentLevelGroupProgress, levelGroupName);
    }

    private void CreateLevelGroupProgress()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int questions = _levelGroupKeeper.LevelGroup.StartQuestions;
        int[] enableEffectIndexes = _effectKeeper.GetEnabledEffectsIndexes().ToArray();
        int[] proposedEffectIndexes = _levelGroupKeeper.LevelGroup.GetCurrentLevelEffectIndexes();

        _currentLevelGroupProgress = new LevelGroupProgress(currentSceneIndex, questions, enableEffectIndexes, proposedEffectIndexes.ToArray());
    }

    [ContextMenu("Reset Progress")]
    private void ResetProgress()
    {
        ResetProgress(_levelGroupKeeper.LevelGroup.name);
    }
}
