using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestarter : MonoBehaviour
{
    [SerializeField] private Ball _ball;

    private void Awake()
    {
        _ball.Died += OnBallDied;
    }

    private void OnBallDied()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
