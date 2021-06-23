using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    [SerializeField] float _rotationSpeed;

    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, _rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.AddQuestion();
            Destroy(gameObject);
        }
    }
}
