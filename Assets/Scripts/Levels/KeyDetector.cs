using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyDetector : MonoBehaviour
{
    public event UnityAction KeyDetected;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Key>() != null)
            KeyDetected?.Invoke();
    }
}
