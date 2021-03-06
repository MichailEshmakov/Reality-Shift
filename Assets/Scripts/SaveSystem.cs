using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveSystem : MonoBehaviour
{
    public event UnityAction ProgressSaved;

    protected void SaveProgress(LevelGroupProgress progress, string levelGroupName)
    {
        PlayerPrefs.SetString(levelGroupName, JsonUtility.ToJson(progress));
        ProgressSaved?.Invoke();
    }

    protected void ResetProgress(string levelGroupName)
    {
        LevelGroupProgress emptyProgress = new LevelGroupProgress();
        SaveProgress(emptyProgress, levelGroupName);
    }

    protected LevelGroupProgress DownloadProgress(string levelGroupName)
    {
        string jsonProgressData = PlayerPrefs.GetString(levelGroupName);
        LevelGroupProgress levelGroupProgress = JsonUtility.FromJson<LevelGroupProgress>(jsonProgressData);
        return levelGroupProgress;
    }
}
