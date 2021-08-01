using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HighScore
{
    [SerializeField] public string playerName;
    [SerializeField] public int score;
}

[Serializable]
public class SaveData
{
    [SerializeField] private List<HighScore> topScoreSaveData;

    public SaveData(List<HighScore> topScoreSaveData)
    {
        this.topScoreSaveData = topScoreSaveData;
    }

    public List<HighScore> GetHighScores()
    {
        return topScoreSaveData;
    }
}