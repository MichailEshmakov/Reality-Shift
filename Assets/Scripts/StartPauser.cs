using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPauser : MonoBehaviour
{
    [SerializeField] private Button _clearSpace;
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private List<GameObject> _menus;

    private void Start()
    {
        Time.timeScale = 0;
        _startMenu.SetActive(true);
    }

    private void OnEnable()
    {
        if (_clearSpace != null)
            _clearSpace.onClick.AddListener(OnClearSpaceClick);
    }

    private void OnDisable()
    {
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
}
