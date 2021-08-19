using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Level Group", menuName = "Level Group", order = 51)]
public class LevelGroup : ScriptableObject
{
    [SerializeField] private int _startQuestions;
    [SerializeField] private List<Level> _levels;
    // TODO: �������� ���

    public int StartQuestions => _startQuestions;

    public int GetCurrentLevelIndex()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        return _levels.FindIndex(level => level.SceneIndex == currentSceneIndex);
    }

    public int GetNextSceneIndex()
    {
        int currentLevelIndex = GetCurrentLevelIndex();
        if (currentLevelIndex == -1 || currentLevelIndex == _levels.Count - 1)
            return -1;
        else
            return _levels[currentLevelIndex + 1].SceneIndex;
    }
}
