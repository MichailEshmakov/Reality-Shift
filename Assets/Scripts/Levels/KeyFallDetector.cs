using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyFallDetector : MonoBehaviour
{
    public event UnityAction KeyKeyFallen;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Key key))
            KeyKeyFallen?.Invoke();
    }
}
