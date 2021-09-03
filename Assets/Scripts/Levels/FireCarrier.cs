using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCarrier : MonoBehaviour
{
    [SerializeField] private Fire _fireTemlate;

    private Fire _currentFire;

    public bool IsBurning => _currentFire != null && _currentFire.gameObject.activeSelf;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out FireGiver fireGiver))
        {
            TakeFire();
        }
    }

    private void TakeFire()
    {
        if (_currentFire == null)
        {
            _currentFire = Instantiate(_fireTemlate, transform.position, transform.rotation);
            _currentFire.Init(transform);
        }
        else
            _currentFire.gameObject.SetActive(true);
    }

}
