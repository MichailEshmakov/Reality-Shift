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
        //_startPosition = FindObjectOfType<StartPosition>().transform.position;
        //_levelBorder = FindObjectOfType<LevelBorder>();
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
        if (Player.Instance != null)
        {
            Player.Awaked -= PlacePlayer;
            if (MainCamera.Instance != null)
            {
                MainCamera.Awaked -= PlacePlayer;
                Vector3 positionDifference = _startPosition - Player.Instance.transform.position;
                Player.Instance.transform.position += positionDifference;
                MainCamera.Instance.transform.position += positionDifference;
                PlayerPlaced?.Invoke();

            }
            else
                MainCamera.Awaked += PlacePlayer;
        }
        else
            Player.Awaked += PlacePlayer;
    }
}
