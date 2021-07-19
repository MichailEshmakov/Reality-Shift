using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KinematicChanger : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _isKinematicChangingOnThisFrame;

    public bool IsKinematicChangingOnThisFrame => _isKinematicChangingOnThisFrame;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public bool TrySetKinematic(bool isKinematic)
    {
        if (gameObject.activeSelf)
        {
            _isKinematicChangingOnThisFrame = true;
            _rigidbody.isKinematic = isKinematic;
            StartCoroutine(ResetChangedKinematicFlag());
            return true;
        }
        else
            return false;
    }

    private IEnumerator ResetChangedKinematicFlag()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        _isKinematicChangingOnThisFrame = false;
    }
}
