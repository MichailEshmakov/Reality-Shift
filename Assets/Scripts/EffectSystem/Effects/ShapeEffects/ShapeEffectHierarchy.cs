
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShapeEffectHierarchy : MonoBehaviour
{
    [SerializeField] private List<ShapeEffect> _shapeEffects;
    [SerializeField] private BallShape _defaultShape;
    [SerializeField] private Transform _ball;
    [SerializeField] private GameObject _currentModel;

    private ShapeEffect _currentEffect;

    public event UnityAction<GameObject> ShapeChanged;

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
                    ApplyShape(_currentEffect.Shape.Template);
                }
                else
                {
                    _currentEffect = null;
                    ApplyShape(_defaultShape.Template);
                }
            }

            shapeEffect.Disabled -= RemoveEffect;
        }
    }

    private void ApplyShape(GameObject shape)
    {
        Destroy(_currentModel);
        _currentModel = Instantiate(shape, _ball);
        ShapeChanged?.Invoke(_currentModel);
    }

    public void AddEffect(ShapeEffect shapeEffect)
    {
        _shapeEffects.Add(shapeEffect);
        ApplyShape(shapeEffect.Shape.Template);
        _currentEffect = shapeEffect;
        shapeEffect.Disabled += RemoveEffect;
    }

    public BallPart[] GetPartsTemplates()
    {
        if (_currentEffect != null)
            return _currentEffect.Shape.PartsTemplates;
        else
            return _defaultShape.PartsTemplates;
    }
}
