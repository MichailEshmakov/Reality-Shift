using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level", order = 51)]
public class Level : ScriptableObject
{
    [SerializeField] private int _sceneIndex;
    [SerializeField] private int[] _effectIndexes;

    public int SceneIndex => _sceneIndex;
    public int[] EffectIndexes => (int[])_effectIndexes.Clone();
    public int[] DeleteIt => _effectIndexes;// TODO: delete
}
