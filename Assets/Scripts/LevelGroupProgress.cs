using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LevelGroupProgress
{
    [SerializeField] private int _sceneIndex;
    [SerializeField] private int _questions;
    [SerializeField] private int[] _enabledEffectIndexes;
    [SerializeField] private int[] _proposedEffectIndexes;
    [SerializeField] private bool _isEffectChosen;

    public int Questions => _questions;
    public int SceneIndex => _sceneIndex;
    public int[] EnabledEffectIndexes => CopyArray(_enabledEffectIndexes);
    public int[] ProposedEffectIndexes => CopyArray(_proposedEffectIndexes);
    public bool IsEffectChosen => _isEffectChosen;

    public LevelGroupProgress(int sceneIndex, int questions, int[] enabledEffectIndexes, int[] proposedEffectIndexes, bool isEffectChosen = false)
    {
        _sceneIndex = sceneIndex;
        _questions = questions;
        _enabledEffectIndexes = enabledEffectIndexes.Distinct().ToArray();
        _proposedEffectIndexes = proposedEffectIndexes.Distinct().ToArray();
        _isEffectChosen = isEffectChosen;
    }

    private int[] CopyArray(int[] previousArray)
    {
        int[] newArray = new int[previousArray.Length];
        Array.Copy(previousArray, newArray, previousArray.Length);
        return newArray;
    }
}
