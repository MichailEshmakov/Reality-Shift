using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    [SerializeField] float _rotationSpeed;

    private void Awake()
    {
        Player.DoWhenAwaked(() => Player.Instance.Died += OnPlayerDied);
    }

    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0, _rotationSpeed * Time.deltaTime, 0);
    }

    private void OnDestroy()
    {
        if (Player.Instance != null)
            Player.Instance.Died -= OnPlayerDied;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.AddQuestion();
            gameObject.SetActive(false);
        }
    }

    private void OnPlayerDied()
    {
        gameObject.SetActive(true);
    }
}
