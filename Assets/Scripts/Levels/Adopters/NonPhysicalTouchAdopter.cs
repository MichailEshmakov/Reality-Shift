using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPhysicalTouchAdopter : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Adoptable child))
        {
            child.AddAdopter(this);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Adoptable child))
        {
            child.RemoveAdopter(this);
        }
    }
}
