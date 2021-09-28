using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class QuestionsPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _questionsText;
    [SerializeField] QuestionScore _questionScore;
    [SerializeField] TestModeSetter _testModeSetter;
    [SerializeField] GameObject _effectsPanel;

    private bool _isSubscribedOnQuestionsChanged = false;
    private Button _button;

    public event UnityAction SubscribedOnQuestionsChanged;
    public bool IsSubscribedOnQuestionsChanged => _isSubscribedOnQuestionsChanged;

    private void Start()
    {
        _button = GetComponent<Button>();
        if (_testModeSetter.IsTestMode)
            _button.onClick.AddListener(OnClick);
        else
            _button.interactable = false;

        _questionScore.QuestionsChanged += OnQuestionsChanged;
        _isSubscribedOnQuestionsChanged = true;
        SubscribedOnQuestionsChanged?.Invoke();
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);
        if (_questionScore != null)
            _questionScore.QuestionsChanged -= OnQuestionsChanged;
    }
    private void OnClick()
    {
        _effectsPanel.SetActive(true);
    }

    public void OnQuestionsChanged(int questions)
    {
        _questionsText.text = questions.ToString();
    }
}
