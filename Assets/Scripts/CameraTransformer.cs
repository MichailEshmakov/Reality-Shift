using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class CameraTransformer<Adder> : MonoBehaviour where Adder : ICameraAdder
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private EffectKeeper _effectKeeper;
    [SerializeField] private TestModeSetter _testModeSetter;

    private Dictionary<Adder, bool> _enabledAddersUpdated;
    private event UnityAction _startParametersSet;
    private bool _isStartParametersSet;

    protected Camera MainCamera => _mainCamera;

    public event UnityAction ParametersReset;
    public event UnityAction AdderEnabled;
    public event UnityAction AdderDisabled;

    protected virtual void Awake()
    {
        if (_testModeSetter.IsTestMode == false)
            _effectKeeper.SavedEffectsEnabled += OnSavedEffectsEnabled;
        else
            OnSavedEffectsEnabled();
    }

    protected virtual void OnDestroy()
    {
        if (_effectKeeper != null)
            _effectKeeper.SavedEffectsEnabled -= OnSavedEffectsEnabled;
    }

    protected void AddAdder(Adder adder)
    {
        _enabledAddersUpdated.Add(adder, false);
    }

    private void OnSavedEffectsEnabled()
    {
        _enabledAddersUpdated = InitAdderBoolDictionary();
    }

    private Dictionary<Adder, bool> InitAdderBoolDictionary()
    {
        List<ICameraAdder> adders = _effectKeeper.GetTypedEffects<ICameraAdder>();
        Dictionary<Adder, bool> addersAdded = new Dictionary<Adder, bool>();
        foreach (ICameraAdder cameraAdder in adders)
        {
            if (cameraAdder is Effect effect)
            {
                effect.Enabled += OnAdderEnabled;
                effect.Disabled += OnAdderDisabled;
                effect.Destroyed += OnAdderDestroyed;
                if (effect.enabled && effect is Adder adder)
                    addersAdded.Add(adder, false);
            }
        }

        return addersAdded;
    }

    private void OnAdderEnabled(Effect effect)
    {
        AdderEnabled?.Invoke();
        if (effect is Adder adder && _enabledAddersUpdated.ContainsKey(adder) == false)
            _enabledAddersUpdated.Add(adder, false);

        if (_isStartParametersSet)
            ResetParameters();
        else
            _startParametersSet += ResetParameters;
    }

    private void OnAdderDisabled(Effect effect)
    {
        AdderDisabled?.Invoke();
        if (effect is Adder adder)
            _enabledAddersUpdated.Remove(adder);

        ResetParameters();
    }

    private void OnAdderDestroyed(Effect adder)
    {
        adder.Enabled -= OnAdderEnabled;
        adder.Disabled -= OnAdderDisabled;
        adder.Destroyed -= OnAdderDestroyed;
    }

    private void ResetParameters()
    {
        ResetTransform();
        ResetFrameParameters();
    }

    protected virtual void ResetFrameParameters()
    {
        foreach (Adder adder in _enabledAddersUpdated.Keys.ToList())
        {
            _enabledAddersUpdated[adder] = false;
        }

        ParametersReset?.Invoke();
    }

    protected abstract void ResetTransform();

    protected void InvokeStartParametersSet()
    {
        _startParametersSet?.Invoke();
    }

    protected bool IsAllAddersUpdated()
    {
        return _enabledAddersUpdated.All(adder => adder.Value);
    }

    protected bool IsAddersContains(Adder key)
    {
        return _enabledAddersUpdated.ContainsKey(key);
    }

    protected void UpdateAdder(Adder adder)
    {
        if (_enabledAddersUpdated.ContainsKey(adder))
            _enabledAddersUpdated[adder] = true;
    }

    public bool HasAdders()
    {
        return _enabledAddersUpdated.Count > 0;
    }
}
