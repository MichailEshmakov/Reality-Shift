using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Level Group", menuName = "Level Group", order = 51)]
public class LevelGroup : ScriptableObject
{
    [SerializeField] private int _startQuestions;
    [SerializeField] private List<Level> _levels;
    [SerializeField] private string _title;
    [SerializeField] private string _number;

    public int StartQuestions => _startQuestions;
    public string Title => _title;
    public string Number => _number;

    public int GetCurrentLevelIndex()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        return _levels.FindIndex(level => level.SceneIndex == currentSceneIndex);
    }

    public int[] GetCurrentLevelEffectIndexes()
    {
         return _levels[GetCurrentLevelIndex()].EffectIndexes;
    }

    public int GetNextSceneIndex()
    {
        int currentLevelIndex = GetCurrentLevelIndex();
        if (currentLevelIndex == -1 || currentLevelIndex == _levels.Count - 1)
            return -1;
        else
            return _levels[currentLevelIndex + 1].SceneIndex;
    }

    public bool HasLevel(int sceneIndex)
    {
        return _levels.Exists(level => level.SceneIndex == sceneIndex);
    }

    public bool TryGetFirstLevelSceneIndex(out int firstLevelSceneIndex)
    {
        if (_levels.Count > 0)
        {
            firstLevelSceneIndex = _levels[0].SceneIndex;
            return true;
        }
        else
        {
            firstLevelSceneIndex = -1;
            return true;
        }
    }
}
