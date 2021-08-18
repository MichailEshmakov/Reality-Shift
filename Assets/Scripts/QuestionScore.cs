using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class QuestionScore : MonoBehaviour
{
    [SerializeField] private int _questions;
    [SerializeField] private Ball _ball;
    [SerializeField] private QuestionsPanel _questionsPanel;

    private int _questionsOnThisLevel;

    public event UnityAction<int> QuestionsChanged;

    private void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        _ball.Died += OnBallDied;
        if (_questionsPanel.IsSubscribedOnQuestionsChanged)
            InvokeQuestionsChanged();
        else
            _questionsPanel.SubscribedOnQuestionsChanged += InvokeQuestionsChanged;
    }

    private void InvokeQuestionsChanged()
    {
        QuestionsChanged?.Invoke(_questions);
    }

    private void OnBallDied()
    {
        _questionsOnThisLevel = 0;
        QuestionsChanged?.Invoke(_questions);
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        _questions += _questionsOnThisLevel;
        _questionsOnThisLevel = 0;
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
