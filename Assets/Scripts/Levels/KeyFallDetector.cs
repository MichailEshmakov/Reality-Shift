using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyFallDetector : MonoBehaviour
{
    public event UnityAction KeyFallen;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Key key))
            KeyFallen?.Invoke();
    }
}
