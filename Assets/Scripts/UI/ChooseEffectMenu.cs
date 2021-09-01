using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseEffectMenu : MonoBehaviour
{
    [SerializeField] private EffectKeeper _effectKeeper;
    [SerializeField] private List<Transform> _chooseEffectButtonPlaces;
    [SerializeField] private ChooseEffectButton _chooseEffectButtonTemplate;
    [SerializeField] private StartMenu _startMenu;
    [SerializeField] private TestModeSetter _testModeSetter;
    [SerializeField] private LevelGroupKeeper _levelGroupKeeper;

    private List<ChooseEffectButton> _chooseEffectButtons;

    private void Awake()
    {
        if (_testModeSetter.IsTestMode == false)
        {
            _chooseEffectButtons = new List<ChooseEffectButton>(_chooseEffectButtonPlaces.Count);
        }
    }

    private void Start()
    {
        if (_testModeSetter.IsTestMode == false)
        {
            if (_effectKeeper.IsSavedEffectsEnabled || _levelGroupKeeper.LevelGroup.GetCurrentLevelIndex() == 0)
                Activate();
            else
                _effectKeeper.SavedEffectsEnabled += Activate;
        }
    }

    private void OnDisable()
    {
        if (_testModeSetter.IsTestMode == false)
        {
            foreach (ChooseEffectButton button in _chooseEffectButtons)
            {
                button.Clicked -= OnChooseEffectButtonClicked;
                Destroy(button.gameObject);
            }

            _chooseEffectButtons.Clear();
            _startMenu.gameObject.SetActive(true);
        }
    }

    private void Activate()
    {
        _effectKeeper.SavedEffectsEnabled -= Activate;
        gameObject.SetActive(true);
        CreateChooseEffectButtons();
    }

    private void CreateChooseEffectButtons()
    {
        List<Effect> randomEffects = _effectKeeper.GetRandomDisabledEffects(_chooseEffectButtonPlaces.Count);
        if (randomEffects.Count > 0)
        {
            for (int i = 0; i < randomEffects.Count; i++)
            {
                ChooseEffectButton chooseEffectButton = Instantiate(_chooseEffectButtonTemplate, _chooseEffectButtonPlaces[i]);
                chooseEffectButton.Init(randomEffects[i]);
                chooseEffectButton.Clicked += OnChooseEffectButtonClicked;
                _chooseEffectButtons.Add(chooseEffectButton);
            }
        }
        else
            gameObject.SetActive(false);
    }

    private void OnChooseEffectButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
