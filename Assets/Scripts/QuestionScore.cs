using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class QuestionScore : Singleton<QuestionScore>
{
    [SerializeField] private int _questions;
    [SerializeField] private Ball _ball;

    private int _questionsOnThisLevel;

    public event UnityAction<int> QuestionsChanged;

    protected override void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        Ball.DoWhenAwaked(() => Ball.Instance.Died += OnBallDied);
        base.Awake();
    }

    private void Start()
    {
        QuestionsChanged?.Invoke(_questions);// TODO: Сделать это, когда панель вопросов подпишется
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
