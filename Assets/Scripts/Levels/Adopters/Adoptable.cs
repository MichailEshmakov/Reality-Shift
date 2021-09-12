using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adoptable : MonoBehaviour
{
    [SerializeField] private Transform _adoptableParent;

    private List<NonPhysicalTouchAdopter> _adopters;
    private Transform _defaultParent;

    private void Start()
    {
        _adopters = new List<NonPhysicalTouchAdopter>();
    }

    private void SetParent(NonPhysicalTouchAdopter adopter)
    {
        Transform settableParent = adopter != null ? adopter.transform : _defaultParent;

        if (_adoptableParent != null)
            _adoptableParent.parent = settableParent;
        else
            transform.parent = settableParent;
    }

    public void AddAdopter(NonPhysicalTouchAdopter adopter)
    {
        if (_adopters.Count == 0)
            _defaultParent = transform.parent;

        SetParent(adopter);
        if (_adopters.Contains(adopter))
            _adopters.Remove(adopter);

        _adopters.Add(adopter);
    }

    public void RemoveAdopter(NonPhysicalTouchAdopter adopter)
    {
        _adopters.Remove(adopter);
        if (_adopters.Count > 0)
            SetParent(_adopters[_adopters.Count - 1]);
        else
            SetParent(null);
    }
}
