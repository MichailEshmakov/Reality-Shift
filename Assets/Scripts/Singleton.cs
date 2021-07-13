using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;
    public static event UnityAction Awaked;

    virtual protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
            Awaked?.Invoke();
        }
        else if (Instance != this as T)
        {
            Debug.Log($"{name} ������ �� ������� ������������");
            Destroy(gameObject);
        }
    }
}
