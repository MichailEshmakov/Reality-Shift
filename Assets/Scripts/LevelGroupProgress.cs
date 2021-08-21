using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LevelGroupProgress
{
    [SerializeField] private int _sceneIndex;
    [SerializeField] private int _questions;
    [SerializeField] private int[] _effectIndexes;

    public int Questions => _questions;
    public int SceneIndex => _sceneIndex;
    public int[] EffectIndexes
    {
        get
        {
            int[] effectIndexes = new int[_effectIndexes.Length];
            Array.Copy(_effectIndexes, effectIndexes, _effectIndexes.Length);
            return effectIndexes;
        }
    }

    public LevelGroupProgress(int sceneIndex, int questions, int[] effectIndexes)
    {
        _sceneIndex = sceneIndex;
        _questions = questions;
        _effectIndexes = effectIndexes;
    }
}
