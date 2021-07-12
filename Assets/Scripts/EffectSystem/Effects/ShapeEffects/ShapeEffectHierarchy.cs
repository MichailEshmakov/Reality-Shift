
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeEffectHierarchy : MonoBehaviour
{
    [SerializeField] private List<ShapeEffect> _shapeEffects;
    [SerializeField] private Mesh _defaultMesh;
    [SerializeField] private MeshCollider _playersCollider;
    [SerializeField] private MeshFilter _playersMeshFilter;

    private ShapeEffect _currentEffect;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
        _playersCollider.sharedMesh = mesh;
        _playersMeshFilter.mesh = mesh;
    }
}
