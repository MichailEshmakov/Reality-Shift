using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class QuestionsPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _questionsText;
    [SerializeField] QuestionScore _questionScore;

    private bool _isSubscribedOnQuestionsChanged = false;

    public event UnityAction SubscribedOnQuestionsChanged;
    public bool IsSubscribedOnQuestionsChanged => _isSubscribedOnQuestionsChanged;

    private void Awake()
    {
        _questionScore.QuestionsChanged += OnQuestionsChanged;
        SubscribedOnQuestionsChanged?.Invoke();
        _isSubscribedOnQuestionsChanged = true;
    }

    private void OnDestroy()
    {
        if (_questionScore != null)
            _questionScore.QuestionsChanged -= OnQuestionsChanged;
    }

    public void OnQuestionsChanged(int questions)
    {
        _questionsText.text = questions.ToString();
    }
}
