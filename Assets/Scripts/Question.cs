using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    [SerializeField] float _rotationSpeed;

    private void Awake()
    {
        Ball.DoWhenAwaked(() => Ball.Instance.Died += OnBallDied);
    }

    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0, _rotationSpeed * Time.deltaTime, 0);
    }

    private void OnDestroy()
    {
        if (Ball.Instance != null)
            Ball.Instance.Died -= OnBallDied;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ball ball))
        {
            QuestionScore.Instance.AddQuestion();
            gameObject.SetActive(false);
        }
    }

    private void OnBallDied()
    {
        gameObject.SetActive(true);
    }
}
