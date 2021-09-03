using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBreaker : MonoBehaviour
{
    [SerializeField] private Rigidbody[] _breakingPartsTemplates;
    [SerializeField] private float _breakingForceCoefficient;
    [SerializeField] private float _partDestroyDelay;

    private void OnCollisionEnter(Collision collision)
    {
        FireCarrier fireCarrier = collision.gameObject.GetComponentInParent<FireCarrier>();
        if (fireCarrier != null && fireCarrier.IsBurning)
            Break(collision.impulse, collision.contacts[0].point);
    }

    private void Break(Vector3 impulse, Vector3 contactPoint)
    {
        float forceCoefficient = 0;
        if (_breakingPartsTemplates.Length > 0)
            forceCoefficient = _breakingForceCoefficient / _breakingPartsTemplates.Length;

        foreach (Rigidbody template in _breakingPartsTemplates)
        {
            Rigidbody part = Instantiate(template, transform.position, transform.rotation);
            part.AddForceAtPosition(- impulse * forceCoefficient, contactPoint, ForceMode.Impulse);
            Destroy(part.gameObject, _partDestroyDelay);
            Destroy(gameObject);
        }
    }
}
