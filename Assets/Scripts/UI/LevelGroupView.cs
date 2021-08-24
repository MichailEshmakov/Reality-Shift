using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelGroupView : SaveSystem
{
    [SerializeField] private LevelGroup _levelGroup;
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_Text _numberLabel;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private ConfirmationWindow _confirmationWindow;
    [SerializeField] private string _confirmationLabelTransparent;

    private string _confirmationLabel;

    private void OnValidate()
    {
        if (_levelGroup != null)
        {
            if (_nameLabel != null)
                _nameLabel.text = _levelGroup.Title;

            if (_numberLabel != null)
                _numberLabel.text = _levelGroup.Number;
        }
    }

    private void Awake()
    {
        _restartButton.onClick.AddListener(OnRestartButtonClick);
        _continueButton.onClick.AddListener(OnContinueButtonClick);
        _confirmationLabel = $"{_confirmationLabelTransparent} \"{_levelGroup.Title}\" ?";
    }

    private void OnDestroy()
    {
        _restartButton.onClick.RemoveListener(RestartLevelGroup);
        _continueButton.onClick.RemoveListener(ContinueLevelGroup);
    }

    private void OnContinueButtonClick()
    {
        ContinueLevelGroup();
    }

    private void OnRestartButtonClick()
    {
        _confirmationWindow.Init(RestartLevelGroup, _confirmationLabel);
    }

    private void ContinueLevelGroup()
    {
        LevelGroupProgress levelGroupProgress = DownloadProgress(_levelGroup.name);
        if (_levelGroup.HasLevel(levelGroupProgress.SceneIndex))
            SceneManager.LoadScene(levelGroupProgress.SceneIndex);
        else if (_levelGroup.TryGetFirstLevelSceneIndex(out int firstLevelSceneIndex))
            SceneManager.LoadScene(firstLevelSceneIndex);
    }

    private void RestartLevelGroup()
    {
        if (_levelGroup.TryGetFirstLevelSceneIndex(out int firstLevelSceneIndex))
        {
            ResetProgress(_levelGroup.name);
            SceneManager.LoadScene(firstLevelSceneIndex);
        } 
    }
}
