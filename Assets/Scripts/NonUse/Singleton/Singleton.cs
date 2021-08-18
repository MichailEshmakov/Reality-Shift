using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;
    public static event UnityAction Awaked;

    private static event UnityAction _onAwakedMethods;

    virtual protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
            Awaked?.Invoke();
            Awaked -= _onAwakedMethods;
            _onAwakedMethods = null;
        }
        else if (Instance != this as T)
        {
            Debug.Log($"{name} удален по причине дублирования");
            Destroy(gameObject);
        }
    }

    public static void DoWhenAwaked(UnityAction method)
    {
        if (Instance != null)
        {
            method?.Invoke();
        }
        else
        {
            _onAwakedMethods += method;
            Awaked += method;
        }
    }
}
