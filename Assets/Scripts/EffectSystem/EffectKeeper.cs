using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectKeeper : Singleton<EffectKeeper>
{
    [SerializeField] private Player _player;
    [SerializeField] private List<Effect> _effects;
    [SerializeField] private Transform _effectMenuContent;
    [SerializeField] private EffectView _effectViewPrefab;

    protected override void Awake()
    {
        foreach (Effect effect in _effects)
        {
            EffectView newView = Instantiate(_effectViewPrefab, _effectMenuContent);
            newView.Init(effect);
            newView.BuyingEffectDisablingTried += OnBuyingEffectDisablingTried;
        }

        base.Awake();
    }

    private void OnBuyingEffectDisablingTried(int price, EffectView view)
    {
        if (_player.TryPayQuestions(price))
        {
            view.OnEffectDisablingBought();
        }
    }

    private void OnDestroy()
    {
        foreach (Effect effect in _effects)
        {
            Destroy(effect);
        }
    }
}
