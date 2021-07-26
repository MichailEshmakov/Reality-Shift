using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionsPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _questionsText;

    private void Awake()
    {
        Player.DoWhenAwaked(() => Player.Instance.QuestionsChanged += OnQuestionsChanged);
    }

    private void OnDestroy()
    {
        if (Player.Instance != null)
            Player.Instance.QuestionsChanged -= OnQuestionsChanged;
    }

    public void OnQuestionsChanged(int questions)
    {
        _questionsText.text = questions.ToString();
    }
}
