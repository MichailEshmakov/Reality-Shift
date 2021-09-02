using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeEffect : Effect
{
    [SerializeField] private BallShape _shape;
    [SerializeField] private ShapeEffectHierarchy _hierarchy;

    public BallShape Shape => _shape;

    protected override void OnEnable()
    {
        base.OnEnable();
        _hierarchy.AddEffect(this);
    }
}
