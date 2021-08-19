using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectKeeper : MonoBehaviour
{
    [SerializeField] private QuestionScore _questionScore;
    [SerializeField] private List<Effect> _effects;
    [SerializeField] private Transform _effectMenuContent;
    [SerializeField] private EffectView _effectViewPrefab;
    [SerializeField] private SaveSystem _saveSystem;
    [SerializeField] private TestModeSetter _testModeSetter;

    private bool _isSavedEffectsEnabled = false;

    public event UnityAction SavedEffectsEnabled;
    public bool IsSavedEffectsEnabled => _isSavedEffectsEnabled;

    private void Start()
    {
        if (_saveSystem.IsProgressDownloaded)
            EnableSavedEffects();
        else
            _saveSystem.ProgressDownloaded += OnProgressDownloaded;

        foreach (Effect effect in _effects)
        {
            EffectView newView = Instantiate(_effectViewPrefab, _effectMenuContent);
            newView.Init(effect, _testModeSetter.IsTestMode);
            newView.BuyingEffectDisablingTried += OnBuyingEffectDisablingTried;
        }
    }

    private void OnDestroy()
    {
        _saveSystem.ProgressDownloaded -= OnProgressDownloaded;
        foreach (Effect effect in _effects)
        {
            Destroy(effect);
        }
    }

    private void OnProgressDownloaded()
    {
        EnableSavedEffects();
    }

    private void EnableSavedEffects()
    {
        int[] savedEffectIndexes = _saveSystem.SavedEffectIndexes;
        foreach (int effectIndex in savedEffectIndexes)
        {
            _effects[effectIndex].enabled = true;
        }

        SavedEffectsEnabled?.Invoke();
    }

    private void OnBuyingEffectDisablingTried(int price, EffectView view)
    {
        if (_questionScore.TryPayQuestions(price))
            view.OnEffectDisablingBought();
    }

    public List<Effect> GetRandomDisabledEffects(int amount)
    {
        List<Effect> result = new List<Effect>(amount);
        List<Effect> disabledEffects = _effects.FindAll(effect => effect.enabled == false);
        for (int i = 0; i < amount; i++)
        {
            if (disabledEffects.Count > 0)
            {
                Effect addingEffect = disabledEffects[Random.Range(0, disabledEffects.Count)];
                disabledEffects.Remove(addingEffect);
                result.Add(addingEffect);
            }
            else
                break;
        }

        return result;
    }

    public List<int> GetEnabledEffectsIndexes()
    {
        List<int> enabledEffectsIndexes = new List<int>();

        for (int i = 0; i < _effects.Count; i++)
        {
            if (_effects[i].enabled)
                enabledEffectsIndexes.Add(i);
        }

        return enabledEffectsIndexes;
    }
}
