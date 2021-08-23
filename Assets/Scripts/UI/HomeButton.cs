using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HomeButton : MonoBehaviour
{
    [SerializeField] private int _mainMenuSceneIndex;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(GoToMainMenu);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(GoToMainMenu);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene(_mainMenuSceneIndex);
    }
}
