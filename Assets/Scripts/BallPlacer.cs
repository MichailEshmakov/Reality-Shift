using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BallPlacer : Singleton<BallPlacer>
{
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private bool _isFirstBallPlacement = true;

    public bool IsFirstBallPlacement => _isFirstBallPlacement;
    public static event UnityAction BallPlaced;

    protected override void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        base.Awake();
        Ball.DoWhenAwaked(() => 
        {
            _startRotation = Ball.Instance.transform.rotation;
            Ball.Instance.Died += OnBallDied;
        });
        
    }

    private void OnDestroy()
    {
        if (Ball.Instance != null)
            Ball.Instance.Died -= OnBallDied;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _isFirstBallPlacement = true;
        _startPosition = FindObjectOfType<StartPosition>().transform.position;
        PlaceBall();
    }

    private void OnBallDied()
    {
        _isFirstBallPlacement = false;
        PlaceBall();
    }

    private void PlaceBall()
    {
        CameraMover.DoWhenAwaked(() => 
        {
            if (CameraMover.Instance.IsStartParametersSet)
            {
                CameraMover.Instance.StartParametersSet -= PlaceBall;
                Ball.DoWhenAwaked(() =>
                {
                    Ball.Instance.transform.position = _startPosition;
                    Ball.Instance.transform.rotation = _startRotation;
                    BallPlaced?.Invoke();
                });
            }
            else
                CameraMover.Instance.StartParametersSet += PlaceBall;
        });
    }
}
