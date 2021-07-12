using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Effect : MonoBehaviour
{
    [SerializeField] private string _label;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _price;

    public string Label => _label;
    public Sprite Icon => _icon;
    public int Price => _price;
    public event UnityAction<Effect> Enabled;
    public event UnityAction<Effect> Disabled;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnEnable()
    {
        Enabled?.Invoke(this);
    }

    protected virtual void OnDisable()
    {
        Disabled?.Invoke(this);
    }
}
