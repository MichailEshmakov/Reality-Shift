using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private QuestionScore _questionScore;
    [SerializeField] private float _rotationSpeed;

    private void Awake()
    {
        _ball.Died += OnBallDied;
    }

    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0, _rotationSpeed * Time.deltaTime, 0);
    }

    private void OnDestroy()
    {
        if (_ball != null)
            _ball.Died -= OnBallDied;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Ball>() != null)
        {
            _questionScore.AddQuestion();
            gameObject.SetActive(false);
        }
    }

    private void OnBallDied()
    {
        gameObject.SetActive(true);
    }
}
