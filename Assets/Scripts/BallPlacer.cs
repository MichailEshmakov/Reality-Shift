using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BallPlacer : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private CameraMover _cameraMover;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private bool _isBallPlaced = false;
    private bool _isFirstBallPlacement = true;

    public bool IsBallPlaced => _isBallPlaced;
    public bool IsFirstBallPlacement => _isFirstBallPlacement;
    public event UnityAction BallPlaced;

    private void Awake()
    {
        _startPosition = FindObjectOfType<StartPosition>().transform.position;
        PlaceBall();
        _startRotation = _ball.transform.rotation;
        _ball.Died += OnBallDied;
        
    }

    private void OnDestroy()
    {
        if (_ball != null)
            _ball.Died -= OnBallDied;
    }

    private void OnBallDied()
    {
        _isFirstBallPlacement = false;
        PlaceBall();
    }

    private void PlaceBall()
    {
        if (_cameraMover.IsStartParametersSet && _ball != null)
        {
            _cameraMover.StartParametersSet -= PlaceBall;
            _ball.transform.position = _startPosition;
            _ball.transform.rotation = _startRotation;
            _isBallPlaced = true;
            BallPlaced?.Invoke();
        }
        else
            _cameraMover.StartParametersSet += PlaceBall;
    }
}
