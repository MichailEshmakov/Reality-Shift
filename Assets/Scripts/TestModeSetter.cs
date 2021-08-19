using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestModeSetter : MonoBehaviour
{
    [SerializeField] private StartMenu _startMenu;
    [SerializeField] private bool _isTestMode;

    public bool IsTestMode => _isTestMode;

    private void Start()
    {
        if (_isTestMode)
            _startMenu.gameObject.SetActive(true);
    }
}
