using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private BallPlacer _ballPlacer;
    [SerializeField] private TestModeSetter _testModeSetter;

    private void Awake()
    {
        if (_testModeSetter.IsTestMode)
        {
            _ballPlacer.BallPlaced += OnBallPlaced;
        }
    }

    private void OnBallPlaced()
    {
        gameObject.SetActive(true);
    }
}
