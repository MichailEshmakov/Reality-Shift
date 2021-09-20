using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPoint : MonoBehaviour
{
    [SerializeField] private float _impulse;

    private Rigidbody _ball;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ball ball) == false 
            && other.TryGetComponent(out Transparanter transparanter) == false
            && other.TryGetComponent(out Question question) == false
            && other.TryGetComponent(out BallPart ballPart) == false
            && other.TryGetComponent(out LevelBorder levelBorder) == false)
        {
            _ball.AddForce((_ball.transform.position - transform.position).normalized * _impulse, ForceMode.Impulse);
        }
    }

    public void Init(Rigidbody ball)
    {
        _ball = ball;
    }
}
