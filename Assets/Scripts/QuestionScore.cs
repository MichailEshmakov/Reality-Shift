using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestionScore : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private QuestionsPanel _questionsPanel;
    [SerializeField] private LevelGroupKeeper _levelGroupKeeper;
    [SerializeField] private LevelSaveSystem _levelSaveSystem;

    private int _questions;
    private int _questionsOnThisLevel;
    private Finish _finish;

    public event UnityAction<int> QuestionsChanged;
    public event UnityAction<int> LevelQuestionsRecorded;

    private void Start()
    {
        SetFinish();
        _ball.Died += OnBallDied;
        
        if (_levelGroupKeeper.LevelGroup.GetCurrentLevelIndex() == 0)
            _questions = _levelGroupKeeper.LevelGroup.StartQuestions;
        else if (_levelSaveSystem.IsProgressDownloaded)
            OnProgressDownloaded();
        else
            _levelSaveSystem.ProgressDownloaded += OnProgressDownloaded;

        TryInvokeQuestionsChanged();
    }

    private void OnDestroy()
    {
        _ball.Died -= OnBallDied;
        _finish.LevelFinished -= OnLevelFinished;
        _levelSaveSystem.ProgressDownloaded -= OnProgressDownloaded;
    }

    private void TryInvokeQuestionsChanged()
    {
        if (_questionsPanel.IsSubscribedOnQuestionsChanged)
        {
            _questionsPanel.SubscribedOnQuestionsChanged -= TryInvokeQuestionsChanged;
            QuestionsChanged?.Invoke(_questions);
        }
        else
            _questionsPanel.SubscribedOnQuestionsChanged += TryInvokeQuestionsChanged;
    }

    private void OnBallDied()
    {
        _questionsOnThisLevel = 0;
        QuestionsChanged?.Invoke(_questions);
    }

    private void OnLevelFinished()
    {
        _questions += _questionsOnThisLevel;
        _questionsOnThisLevel = 0;
        LevelQuestionsRecorded?.Invoke(_questions);
    }

    private void SetFinish()
    {
        _finish = FindObjectOfType<Finish>();
        if (_finish == null)
            Debug.LogError($"Finish is not found by {gameObject.name}");
        else
            _finish.LevelFinished += OnLevelFinished;
    }

    private void OnProgressDownloaded()
    {
        _questions = _levelSaveSystem.CurrentLevelGroupProgress.Questions;
    }

    public void AddQuestion()
    {
        _questionsOnThisLevel++;
        QuestionsChanged?.Invoke(_questions + _questionsOnThisLevel);
    }

    public bool TryPayQuestions(int price)
    {
        if (_questions >= price)
        {
            _questions -= price;
            QuestionsChanged?.Invoke(_questions + _questionsOnThisLevel);
            return true;
        }

        return false;
    }
}
