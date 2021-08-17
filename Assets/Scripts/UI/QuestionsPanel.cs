using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionsPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _questionsText;

    private void Awake()
    {
        QuestionScore.DoWhenAwaked(() => QuestionScore.Instance.QuestionsChanged += OnQuestionsChanged);
    }

    private void OnDestroy()
    {
        if (QuestionScore.Instance != null)
            QuestionScore.Instance.QuestionsChanged -= OnQuestionsChanged;
    }

    public void OnQuestionsChanged(int questions)
    {
        _questionsText.text = questions.ToString();
    }
}
