using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChooseEffectMenu : MonoBehaviour
{
    [SerializeField] private EffectKeeper _effectKeeper;
    [SerializeField] private List<Transform> _chooseEffectButtonPlaces;
    [SerializeField] private ChooseEffectButton _chooseEffectButtonTemplate;
    [SerializeField] private StartMenu _startMenu;
    [SerializeField] private TestModeSetter _testModeSetter;
    [SerializeField] private LevelGroupKeeper _levelGroupKeeper;
    [SerializeField] private LevelSaveSystem _levelSaveSystem;

    private List<ChooseEffectButton> _chooseEffectButtons;
    private List<Effect> _proposedEffects;

    public event UnityAction<List<Effect>> EffectChosen;

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
            if (_levelSaveSystem.IsProgressSet == false)
                _levelSaveSystem.ProgressSet += OnProgressDownloaded;
            else
                OnProgressDownloaded();
        }
    }

    private void OnProgressDownloaded()
    {
        _levelSaveSystem.ProgressSet -= OnProgressDownloaded;
        if (_levelSaveSystem.CurrentLevelGroupProgress.IsEffectChosen == false)
        {
            _proposedEffects = _effectKeeper.GetProposedEffects();
            CreateChooseEffectButtons();
        }
        else
            gameObject.SetActive(false);
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

    private void CreateChooseEffectButtons()
    {
        if (_proposedEffects.Count > 0)
        {
            for (int i = 0; i < _proposedEffects.Count; i++)
            {
                ChooseEffectButton chooseEffectButton = Instantiate(_chooseEffectButtonTemplate, _chooseEffectButtonPlaces[i]);
                chooseEffectButton.Init(_proposedEffects[i]);
                chooseEffectButton.Clicked += OnChooseEffectButtonClicked;
                _chooseEffectButtons.Add(chooseEffectButton);
            }
        }
        else
            gameObject.SetActive(false);
    }

    private void OnChooseEffectButtonClicked(Effect effect)
    {
        _proposedEffects.Remove(effect);
        EffectChosen(_proposedEffects);
        gameObject.SetActive(false);
    }
}
