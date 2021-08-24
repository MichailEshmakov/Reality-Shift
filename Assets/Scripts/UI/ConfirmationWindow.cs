using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ConfirmationWindow : MonoBehaviour
{
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;
    [SerializeField] private TMP_Text _label;

    private UnityAction _confirmatingAction;

    private void Awake()
    {
        _noButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        _yesButton.onClick.RemoveListener(_confirmatingAction);
        _confirmatingAction = null;
    }

    private void OnDestroy()
    {
        _noButton.onClick.RemoveListener(Close);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    public void Init(UnityAction confirmatingAction, string lablelText)
    {
        gameObject.SetActive(true);
        _confirmatingAction = confirmatingAction;
        _yesButton.onClick.AddListener(_confirmatingAction);
        _label.text = lablelText;
    }
}
