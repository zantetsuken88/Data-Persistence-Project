using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreScreen : MonoBehaviour
{
    [SerializeField] private GameObject pfHighScoreTextLeft;
    [SerializeField] private GameObject pfHighScoreTextRight;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private Button mainMenuButon;
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateHighScoreScreen();
        mainMenuButon.onClick.AddListener(delegate { PlayerManager.Instance.LoadMainMenu(); });
    }

    private void GenerateHighScoreScreen()
    {
        List<HighScore> highScores = PlayerManager.Instance.highScores;
        int highScoresCount = highScores.Count;
    
        for (int i = 0; i < highScoresCount; i++)
        {
            bool isLeft = i < 5;
            float offset = isLeft ? i * -60f : (i - 5) * -60f;
            GenerateHighScoreText(isLeft, offset, highScores[i], i + 1);
        }
    }

    private void GenerateHighScoreText(bool isLeft, float offsetY, HighScore highScore, int rank)
    {
        GameObject pf = isLeft ? pfHighScoreTextLeft : pfHighScoreTextRight;
        GameObject highScoreText = Instantiate(pf, uiPanel.transform, false);
        highScoreText.transform.position += Vector3.up * offsetY; 
        highScoreText.GetComponent<TextMeshProUGUI>().text = SetHighScoreText(rank, highScore.playerName, highScore.score);
    }

    private string SetHighScoreText(int rank, string playerName, int score)
    {
        return $"[{rank}] [{playerName}] [{score}]";
    }
}
