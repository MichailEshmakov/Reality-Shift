using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Transparanter : MonoBehaviour
{
    [SerializeField] private List<Effect> _offsetChangingEffects;
    [Range(0, 1)] [SerializeField] private float _transparentAlpha;

    private CapsuleCollider _collider;
    private bool _isAnyoOffsetChangingEffectEnable;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
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
        if (other.gameObject.TryGetComponent(out Player player) == false && other.gameObject.TryGetComponent(out MeshRenderer renderer))
            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, _transparentAlpha);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MeshRenderer renderer))
            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1);
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
