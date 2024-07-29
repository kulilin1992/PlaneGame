using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistenSingleton<ScoreManager>
{
    public int Score => score;
    int score;
    int currentScore;

    Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);


    #region SCORE_DISPLAY
    public void ResetScore()
    {
        score = 0;
        currentScore = 0;
        ScoreDisplay.UpdateText(score);
    }

    public void AddScore(int scorePoint)
    {
        currentScore += scorePoint;
        StartCoroutine(nameof(AddScoreCoroutine));
    }

    IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.ScaleText(scoreTextScale);
        while (score < currentScore)
        {
            score += 1;
            ScoreDisplay.UpdateText(score);
            yield return null;
        }

        ScoreDisplay.ScaleText(Vector3.one);
    }
    #endregion


    #region HIGH_SCORE_SYSTEM
    [System.Serializable] public class PlayerScore
    {
        public int score;
        public string name;
        public PlayerScore(int score, string name) => (this.score, this.name) = (score, name);
    }


    [System.Serializable] public class PlayerScoreData
    {
        public List<PlayerScore> playerScoresList = new List<PlayerScore>();
    }


    readonly string SaveFileName = "player_score.json";
    string playerName = "No Name";

    public bool HasNewHighScore => score > LoadPlayerScoreData().playerScoresList[9].score;
    public PlayerScoreData LoadPlayerScoreData()
    {
        var playerScoreData = new PlayerScoreData();
        if (SaveSystem.SaveFileExists(SaveFileName)) {
            playerScoreData = SaveSystem.Load<PlayerScoreData>(SaveFileName);
        }
        else
        {
            while (playerScoreData.playerScoresList.Count < 10)
            {
                playerScoreData.playerScoresList.Add(new PlayerScore(0, playerName));
            }
            SaveSystem.Save(SaveFileName, playerScoreData);
        }

        return playerScoreData;
    }

    public void SavePlayerScoreData()
    {
        var playerScoreData = LoadPlayerScoreData();
        playerScoreData.playerScoresList.Add(new PlayerScore(score, playerName));

        playerScoreData.playerScoresList.Sort((a, b) => b.score.CompareTo(a.score));

        SaveSystem.Save(SaveFileName, playerScoreData);
    }

    public void SetPlayerName(string name) => playerName = name;

    #endregion
}
