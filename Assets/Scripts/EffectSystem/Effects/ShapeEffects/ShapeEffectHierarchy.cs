
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeEffectHierarchy : Singleton<ShapeEffectHierarchy>
{
    [SerializeField] private List<ShapeEffect> _shapeEffects;
    [SerializeField] private Mesh _defaultMesh;

    private MeshCollider _ballsCollider;
    private MeshFilter _ballsMeshFilter;
    private ShapeEffect _currentEffect;

    protected override void Awake()
    {
        base.Awake();
        Ball.DoWhenAwaked(TakeBallComponents);
    }

    public void AddEffect(ShapeEffect shapeEffect)
    {
        _shapeEffects.Add(shapeEffect);
        ApplyShape(shapeEffect.Mesh);
        _currentEffect = shapeEffect;
        shapeEffect.Disabled += RemoveEffect;
    }

    private void RemoveEffect(Effect effect)
    {
        ShapeEffect shapeEffect = effect as ShapeEffect;
        if (shapeEffect != null && _shapeEffects.Remove(shapeEffect))
        {
            if (shapeEffect == _currentEffect)
            {
                if (_shapeEffects.Count > 0)
                {
                    _currentEffect = _shapeEffects[_shapeEffects.Count - 1];
                    ApplyShape(_currentEffect.Mesh);
                }
                else
                {
                    _currentEffect = null;
                    ApplyShape(_defaultMesh);
                }
            }

            shapeEffect.Disabled -= RemoveEffect;
        }
    }

    private void ApplyShape(Mesh mesh)
    {
        if (_ballsCollider != null)
            _ballsCollider.sharedMesh = mesh;
        if (_ballsMeshFilter != null)
            _ballsMeshFilter.mesh = mesh;
    }

    private void TakeBallComponents()
    {
        _ballsCollider = Ball.Instance.GetComponent<MeshCollider>();
        _ballsMeshFilter = Ball.Instance.GetComponent<MeshFilter>();
    }
}
