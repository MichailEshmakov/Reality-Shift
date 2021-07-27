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
        base.Awake();
        Player.DoWhenAwaked(() => Player.Instance.Died += OnPlayerDied);
        PlacePlayer();
    }

    private void OnDestroy()
    {
        if (Player.Instance != null)
            Player.Instance.Died -= OnPlayerDied;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _startPosition = FindObjectOfType<StartPosition>().transform.position;
        PlacePlayer();
    }

    private void OnPlayerDied()
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
