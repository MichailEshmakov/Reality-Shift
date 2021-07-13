using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeEffect : Effect
{
    [SerializeField] private Mesh _mesh;

    public Mesh Mesh => _mesh;

    protected override void OnEnable()
    {
        base.OnEnable();
        ShapeEffectHierarchy.Instance.AddEffect(this);
    }
}
