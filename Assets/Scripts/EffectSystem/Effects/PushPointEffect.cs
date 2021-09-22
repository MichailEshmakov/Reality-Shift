using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPointEffect : Effect
{
    [SerializeField] private Ball _ball;
    [SerializeField] private PushPoint _pushPointTemplate;

    private PushPoint _pushPoint;
    private Rigidbody _ballRigidbody;

    private void Awake()
    {
        _ballRigidbody = _ball.GetComponent<Rigidbody>();
    }

    protected override void OnEnable()
    {
        _pushPoint = Instantiate(_pushPointTemplate, _ball.transform);
        _pushPoint.Init(_ballRigidbody);
        SetPushPoint();
        _ball.ShapeChanged += OnShapeChanged;
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        Destroy(_pushPoint.gameObject);
        _ball.ShapeChanged -= OnShapeChanged;
        base.OnDisable();
    }

    private void OnShapeChanged()
    {
        SetPushPoint();
    }

    private void SetPushPoint()
    {
        _pushPoint.transform.localPosition = _ball.Model.GetComponent<MeshFilter>().mesh.vertices[0];
    }
}
