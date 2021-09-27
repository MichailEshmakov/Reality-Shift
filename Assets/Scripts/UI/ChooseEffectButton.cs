using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class ChooseEffectButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private Image _icon;

    private Button _button;
    private Effect _effect;

    public event UnityAction<Effect> Clicked;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _effect.enabled = true;
        Clicked?.Invoke(_effect);
    }

    public void Init(Effect effect)
    {
        _effect = effect;
        _label.text = _effect.Label;
        _icon.sprite = _effect.Icon;
    }
}
