using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EffectView : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _disableButton;
    [SerializeField] private Button _enableButton;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private bool _isTestMode;

    private Effect _effect;

    public UnityAction<int, EffectView> BuyingEffectDisablingTried;

    public void Init(Effect effect)
    {
        _effect = effect;
        _label.text = _effect.Label;
        if (_effect.Icon != null)
            _icon.sprite = _effect.Icon;

        _priceText.text = _effect.Price.ToString();

        _effect.Enabled += OnEffectEnabled;
        _effect.Disabled += OnEffectDisabled;

        _disableButton.onClick.AddListener(TryBuyEffectDisabling);
        _enableButton.onClick.AddListener(EnableEffect);

        SetButtonsInteractibling();
        gameObject.SetActive(_effect.enabled || _isTestMode);
    }

    public void TryBuyEffectDisabling()
    {
        if (_effect.enabled)
            BuyingEffectDisablingTried?.Invoke(_effect.Price, this);
        else
            SetButtonsInteractibling();
    }

    public void OnEffectDisablingBought()
    {
        _effect.enabled = false;
    }

    private void EnableEffect()
    {
        _effect.enabled = true;
    }

    private void OnEffectEnabled()
    {
        gameObject.SetActive(true);
        SetButtonsInteractibling();
    }

    private void OnEffectDisabled()
    {
        gameObject.SetActive(_isTestMode);
        SetButtonsInteractibling();
    }

    private void OnDestroy()
    {
        _effect.Enabled -= OnEffectEnabled;
        _effect.Disabled -= OnEffectDisabled;
        _disableButton.onClick.RemoveListener(TryBuyEffectDisabling);
        _disableButton.onClick.RemoveListener(EnableEffect);
    }

    private void SetButtonsInteractibling()
    {
        _enableButton.interactable = (_effect.enabled == false) && _isTestMode;
        _disableButton.interactable = _effect.enabled;
    }
}
