using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeEffect : Effect
{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private ShapeEffectHierarchy _hierarchy;

    public Mesh Mesh => _mesh;

    protected override void OnEnable()
    {
        base.OnEnable();
        _hierarchy.AddEffect(this);
    }
}
