using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ball Form", menuName = "Ball Form", order = 51)]
public class BallShape : ScriptableObject
{
    [SerializeField] private GameObject _formTemplate;
    [SerializeField] private BallPart[] _partsTemplates;

    public GameObject Template => _formTemplate;
    public BallPart[] PartsTemplates
    {
        get
        {
            BallPart[] partsTemplates = new BallPart[_partsTemplates.Length];
            Array.Copy(_partsTemplates, partsTemplates, _partsTemplates.Length);
            return partsTemplates;
        }
    }
}
