using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebuilder : MonoBehaviour
{
    private List<Vector3> _childrenStartLocalPositions;
    private List<Quaternion> _childrenStartLocalRotations;
    private List<Rigidbody> _childRigidbodies;
    // TODO: учесть, что дети могут покидать родителя, содать скрипт ребенка и подписаться на его события

    private void OnEnable()
    {
        Rebuild();
    }

    private void Start()
    {
        _childrenStartLocalPositions = new List<Vector3>(transform.childCount);
        _childrenStartLocalRotations = new List<Quaternion>(transform.childCount);
        _childRigidbodies = new List<Rigidbody>(transform.childCount);

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Transform child = transform.GetChild(i);
            _childrenStartLocalPositions.Add(child.localPosition);
            _childrenStartLocalRotations.Add(child.localRotation);
            _childRigidbodies.Add(child.GetComponent<Rigidbody>());
        }
    }


    private void Rebuild()
    {
        if (_childrenStartLocalPositions != null 
            && _childrenStartLocalPositions.Count > 0 
            && _childrenStartLocalRotations != null
            && _childrenStartLocalRotations.Count == _childrenStartLocalPositions.Count)
        {
            Debug.Log("Rebuild");
            for (int i = 0; i < transform.childCount - 1; i++)
            {
                Transform child = transform.GetChild(i);
                child.localPosition = _childrenStartLocalPositions[i];
                child.localRotation = _childrenStartLocalRotations[i];
                if (_childRigidbodies[i] != null)
                {
                    _childRigidbodies[i].velocity = Vector3.zero;
                    _childRigidbodies[i].angularVelocity = Vector3.zero;
                }
                    
            }
        }
    }
}
