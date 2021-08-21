using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelGroupView : MonoBehaviour
{
    [SerializeField] private LevelGroup _levelGroup;
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_Text _numberLabel;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _continueButton;

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
        _restartButton.onClick.AddListener(RestartLevelGroup);
        _continueButton.onClick.AddListener(ContinueLevelGroup);
    }

    private void OnDestroy()
    {
        _restartButton.onClick.RemoveListener(RestartLevelGroup);
        _continueButton.onClick.RemoveListener(ContinueLevelGroup);
    }

    private void ContinueLevelGroup()
    {
        string levelGroupJsonData = PlayerPrefs.GetString(_levelGroup.name);
        LevelGroupProgress levelGroupProgress = JsonUtility.FromJson<LevelGroupProgress>(levelGroupJsonData);
        if (_levelGroup.HasLevel(levelGroupProgress.SceneIndex))
            SceneManager.LoadScene(levelGroupProgress.SceneIndex);
        else if (_levelGroup.TryGetFirstLevelSceneIndex(out int firstLevelSceneIndex))
            SceneManager.LoadScene(firstLevelSceneIndex);
        //TODO: Добавить кнопку выхода из уровня
    }

    private void RestartLevelGroup()
    {
    }
}
