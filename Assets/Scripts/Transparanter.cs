using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Transparanter : MonoBehaviour
{
    [SerializeField] private List<Effect> _offsetChangingEffects;
    [SerializeField] private Material _transparentMaterial;

    private CapsuleCollider _collider;
    private bool _isAnyoOffsetChangingEffectEnable;
    private Dictionary<GameObject, Material> _transparentedObjects;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _transparentedObjects = new Dictionary<GameObject, Material>();
        Player.DoWhenAwaked(() =>
        {
            MainCamera.DoWhenAwaked(() =>
            {
                SetColliderSize(Player.Instance.transform.position - MainCamera.Instance.transform.position);
            });
        });
        foreach (Effect effect in _offsetChangingEffects)
        {
            effect.Enabled += OnOffsetChangingEffectEnabled;
            effect.Disabled += OnOffsetChangingEffectDisabled;
        }
    }

    private void FixedUpdate()
    {
        Vector3 fromCameratoPlayer = Player.Instance.transform.position - MainCamera.Instance.transform.position;
        transform.rotation = Quaternion.LookRotation(fromCameratoPlayer);
        if (_isAnyoOffsetChangingEffectEnable)
            SetColliderSize(fromCameratoPlayer);
    }

    private void OnDestroy()
    {
        foreach (Effect effect in _offsetChangingEffects)
        {
            effect.Enabled -= OnOffsetChangingEffectEnabled;
            effect.Disabled -= OnOffsetChangingEffectDisabled;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckTransparantable(other.gameObject, out MeshRenderer renderer))
        {
            Material newMaterial = new Material(_transparentMaterial)
            {
                color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, _transparentMaterial.color.a)
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

    private bool CheckTransparantable(GameObject checkingObject, out MeshRenderer renderer)
    {
        renderer = null;
        return checkingObject.TryGetComponent(out Player player) == false
            && checkingObject.TryGetComponent(out Question question) == false
            && checkingObject.TryGetComponent(out renderer);
    }

    private void OnOffsetChangingEffectDisabled(Effect arg0)
    {
        SetOffsetChangingEffectEnablingFlag();
    }

    private void OnOffsetChangingEffectEnabled(Effect arg0)
    {
        SetOffsetChangingEffectEnablingFlag();
    }

    private void SetOffsetChangingEffectEnablingFlag()
    {
        _isAnyoOffsetChangingEffectEnable = _offsetChangingEffects.Any(effect => effect.enabled);
    }

    private void SetColliderSize(Vector3 fromCameratoPlayer)
    {
        _collider.height = fromCameratoPlayer.magnitude;
        _collider.center = new Vector3(_collider.center.x, _collider.center.y, _collider.height / 2);
    }
}
