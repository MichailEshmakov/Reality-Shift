using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseEffectMenu : MonoBehaviour
{
    [SerializeField] private EffectKeeper _effectKeeper;
    [SerializeField] private List<Transform> _chooseEffectButtonPlaces;
    [SerializeField] private ChooseEffectButton _chooseEffectButtonTemplate;
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private BallPlacer _ballPlacer;

    private List<ChooseEffectButton> _chooseEffectButtons;

    private void Awake()
    {
        _chooseEffectButtons = new List<ChooseEffectButton>(_chooseEffectButtonPlaces.Count);
        _ballPlacer.BallPlaced += OnBallPlaced;
    }

    private void OnDisable()
    {
        foreach (ChooseEffectButton button in _chooseEffectButtons)
        {
            button.Clicked -= OnChooseEffectButtonClicked;
            Destroy(button.gameObject);
        }

        _chooseEffectButtons.Clear();
        _startMenu.SetActive(true);
    }

    private void OnDestroy()
    {
        _ballPlacer.BallPlaced -= OnBallPlaced;
    }

    private void OnBallPlaced()
    {
        if (_ballPlacer.IsFirstBallPlacement)
        {
            gameObject.SetActive(true);
            CreateChooseEffectButtons();
        }
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
