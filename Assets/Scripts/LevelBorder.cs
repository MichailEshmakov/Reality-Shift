using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelBorder : MonoBehaviour
{
    public event UnityAction PlayerOuted;

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            PlayerOuted?.Invoke();
        }
    }
}
