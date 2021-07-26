using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerPlacer : Singleton<PlayerPlacer>
{
    private Vector3 _startPosition;
    private LevelBorder _levelBorder;

    public static event UnityAction PlayerPlaced;

    protected override void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded; ;
        base.Awake();
        PlacePlayer();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _levelBorder = FindObjectOfType<LevelBorder>();
        if (_levelBorder != null)
            _levelBorder.PlayerOuted += OnPlayerOuted;

        _startPosition = FindObjectOfType<StartPosition>().transform.position;
        PlacePlayer();
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        if (_levelBorder != null)
        {
            _levelBorder.PlayerOuted -= OnPlayerOuted;
            _levelBorder = null;
        }
    }

    private void OnPlayerOuted()
    {
        PlacePlayer();
    }

    private void PlacePlayer()
    {
        CameraMover.DoWhenAwaked(() => 
        {
            if (CameraMover.Instance.IsStartParametersSet)
            {
                CameraMover.Instance.StartParametersSet -= PlacePlayer;
                Player.DoWhenAwaked(() =>
                {
                    Vector3 positionDifference = _startPosition - Player.Instance.transform.position;
                    Player.Instance.transform.position += positionDifference;
                    PlayerPlaced?.Invoke();
                });
            }
            else
                CameraMover.Instance.StartParametersSet += PlacePlayer;
        });
    }
}
