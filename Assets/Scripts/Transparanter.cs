using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Transparanter : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Material _transparentMaterial;
    [SerializeField] private EffectKeeper _effectKeeper;

    private CapsuleCollider _collider;
    private Dictionary<GameObject, Material> _transparentedObjects;
    private List<CameraTransformingEffect> _enabledMovingEffects;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _transparentedObjects = new Dictionary<GameObject, Material>();
    }

    private void Start()
    {
        InitCameraMovingEffectsList();
        StartCoroutine(UpdateMoving());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckTransparantable(other.gameObject, out MeshRenderer renderer) && _transparentedObjects.ContainsKey(other.gameObject) == false)
        {
            Material newMaterial = new Material(_transparentMaterial)
            {
                color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, _transparentMaterial.color.a),
                mainTexture = renderer.material.mainTexture
            };
            
            _transparentedObjects.Add(other.gameObject, renderer.material);
            renderer.material = newMaterial;
        }            
    }

    private void OnTriggerExit(Collider other)
    {
        if (_transparentedObjects.TryGetValue(other.gameObject, out Material previousMaterial))
        {
            _transparentedObjects.Remove(other.gameObject);
            if (other.gameObject.TryGetComponent(out MeshRenderer renderer))
                renderer.material = previousMaterial;
        }
    }

    private void InitCameraMovingEffectsList()
    {
        List<CameraTransformingEffect> cameraMovingEffects = _effectKeeper.GetTypedEffects<CameraTransformingEffect>();
        _enabledMovingEffects = new List<CameraTransformingEffect>();
        foreach (CameraTransformingEffect effect in cameraMovingEffects)
        {
            if (effect.enabled)
                _enabledMovingEffects.Add(effect);
            effect.Enabled += OnCameraMovingEffectEnabled;
            effect.Disabled += OnCameraMovingEffectDisabled;
            effect.Destroyed += OnCameraMovingEffectDestroyed;
        }
    }

    private IEnumerator UpdateMoving()
    {
        do
        {
            yield return new WaitForFixedUpdate();
            Vector3 fromCameratoBal = _ball.transform.position - _mainCamera.transform.position;
            transform.rotation = Quaternion.LookRotation(fromCameratoBal);
            SetColliderSize(fromCameratoBal);
        }
        while (_enabledMovingEffects.Count > 0);
    }

    private void OnCameraMovingEffectEnabled(Effect effect)
    {
        if (effect is CameraTransformingEffect movingEffect && _enabledMovingEffects.Contains(movingEffect) == false)
        {
            _enabledMovingEffects.Add(movingEffect);
            if (_enabledMovingEffects.Count == 1)
                StartCoroutine(UpdateMoving());
        }
    }

    private void OnCameraMovingEffectDisabled(Effect effect)
    {
        if (effect is CameraTransformingEffect movingEffect)
            _enabledMovingEffects.Remove(movingEffect);
    }

    private void OnCameraMovingEffectDestroyed(Effect effect)
    {
        effect.Enabled -= OnCameraMovingEffectEnabled;
        effect.Disabled -= OnCameraMovingEffectDisabled;
        effect.Destroyed -= OnCameraMovingEffectDestroyed;
    }

    private bool CheckTransparantable(GameObject checkingObject, out MeshRenderer renderer)
    {
        renderer = null;
        return checkingObject.GetComponentInParent<Ball>() == null
            && checkingObject.TryGetComponent(out Question question) == false
            && checkingObject.TryGetComponent(out BallPart ballPart) == false
            && checkingObject.TryGetComponent(out renderer);
    }

    private void SetColliderSize(Vector3 fromCameratoBal)
    {
        Vector3 fromCameratoBall = _ball.transform.position - _mainCamera.transform.position;
        _collider.height = fromCameratoBall.magnitude;
        _collider.center = new Vector3(_collider.center.x, _collider.center.y, _collider.height / 2);
    }
}
