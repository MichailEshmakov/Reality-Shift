using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class StartPlayerPlacer : Singleton<StartPlayerPlacer>
{
    private Vector3 _startPosition;

    public static event UnityAction PlayerPlaced;

    protected override void Awake()
    {
        _startPosition = FindObjectOfType<StartPosition>().transform.position;
        SceneManager.sceneLoaded += OnSceneLoaded;
        base.Awake();
        PlacePlayer();

    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _startPosition = FindObjectOfType<StartPosition>().transform.position;
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
