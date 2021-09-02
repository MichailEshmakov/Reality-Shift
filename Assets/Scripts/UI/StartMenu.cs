using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private TestModeSetter _testModeSetter;

    private void Start()
    {
        if (_testModeSetter.IsTestMode)
        {
            gameObject.SetActive(true);
        }
    }
}
