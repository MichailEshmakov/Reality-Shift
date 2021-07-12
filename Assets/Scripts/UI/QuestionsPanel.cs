using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionsPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _questionsText;
    [SerializeField] Player _player;
    [SerializeField] GameObject _effectListPanel;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        _player.QuestionsChanged += OnQuestionsChanged;
    }

    private void OnDisable()
    {
        _player.QuestionsChanged -= OnQuestionsChanged;
    }

    public void OnQuestionsChanged(int questions)
    {
        _questionsText.text = questions.ToString();
    }
}
