using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPauser : Singleton<StartPauser>
{
    [SerializeField] private Button _clearSpace;
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private List<GameObject> _menus;

    protected override void Awake()
    {
        base.Awake();
        BallPlacer.BallPlaced += OnBallPlaced;
        if (_clearSpace != null)
            _clearSpace.onClick.AddListener(OnClearSpaceClick);
    }

    private void OnDestroy()
    {
        BallPlacer.BallPlaced -= OnBallPlaced;
        if (_clearSpace != null)
            _clearSpace.onClick.RemoveListener(OnClearSpaceClick);
    }

    private void OnClearSpaceClick()
    {    
        if (_menus.Exists(menu => menu.activeSelf) == false)
        {
            Time.timeScale = 1;
            if (_startMenu != null)
                _startMenu.SetActive(false);
        }
    }

    private void OnBallPlaced()
    {
        Time.timeScale = 0;
        if (BallPlacer.Instance.IsFirstBallPlacement == false)
            _startMenu.SetActive(true);
    }
}
