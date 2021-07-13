using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionsPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _questionsText;

    private void Awake()
    {
        if (Player.Instance == null)
            Player.Awaked += OnPlayerAwaked;
        else
            Player.Instance.QuestionsChanged += OnQuestionsChanged;
    }

    private void OnPlayerAwaked()
    {
        Player.Instance.QuestionsChanged += OnQuestionsChanged;
    }

    private void OnDestroy()
    {
        Player.Instance.QuestionsChanged -= OnQuestionsChanged;
    }

    public void OnQuestionsChanged(int questions)
    {
        _questionsText.text = questions.ToString();
    }
}
