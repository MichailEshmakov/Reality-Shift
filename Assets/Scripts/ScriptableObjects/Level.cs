using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level", order = 51)]
public class Level : ScriptableObject
{
    [SerializeField] private int _sceneIndex;
    // TODO: Добавить эффекты

    public int SceneIndex => _sceneIndex;
}
