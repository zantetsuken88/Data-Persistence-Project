using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private TextMeshProUGUI highScoreText;

    public string playerName;

    [SerializeField] public List<HighScore> highScores;


    public Action OnNewHighScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        } else Destroy(gameObject);
    }

    private void Start()
    {
        LoadHighScores();
        playerNameInputField.onValueChanged.AddListener(SetPlayerName);
        OnNewHighScore += SaveHighScores;

        if (highScores.Count > 0)
        {
            var topScore = GetTopScore();
            highScoreText.text = $"Top Score: {topScore.playerName} [ {topScore.score} ]";
        }
    }

    public void SetPlayerName(string nameInput)
    {
        playerName = nameInput;
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadHighScoreScreen()
    {
        SceneManager.LoadScene("HighScoreScreen");
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
        
        SaveHighScores();
    }

    public HighScore GetTopScore()
    {
        SortHighScores();
        return highScores[0];
    }

    public void AddHighScore(string highScorePlayerName, int score)
    {
        HighScore highScore = new HighScore {playerName = highScorePlayerName, score = score};
        highScores.Add(highScore);
        
        SortHighScores();
        
        OnNewHighScore?.Invoke();
    }

    private void SortHighScores()
    {
        highScores.Sort((a, b) => b.score.CompareTo(a.score));
        CullHighScores();
    }

    private void CullHighScores()
    {
        int maxHighScores = 10;
        int currentCount = highScores.Count;
        
        if (currentCount > maxHighScores)
        {
            highScores.RemoveRange(maxHighScores, currentCount - maxHighScores);
        }
    }

    private void SaveHighScores()
    {
        SaveData saveData = new SaveData(highScores);
        string savePath = Application.persistentDataPath + "savefile.json";
        string highScoreData = JsonUtility.ToJson(saveData);
        
        File.WriteAllText(savePath, highScoreData);
    }

    private void LoadHighScores()
    {
        string savePath = Application.persistentDataPath + "savefile.json";

        if (File.Exists(savePath))
        {
            string highScoreJson = File.ReadAllText(savePath);
            
            SaveData saveData = JsonUtility.FromJson<SaveData>(highScoreJson);

            highScores = saveData.GetHighScores();
        }
    }
}
