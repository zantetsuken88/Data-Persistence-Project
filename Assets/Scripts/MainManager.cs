using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    [SerializeField] private TextMeshProUGUI topScoreText;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button highScoresButton;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private string playerName;
    private HighScore currentTopScore;

    private Action OnGameOver;

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        if (PlayerManager.Instance != null)
        {
            playerName = PlayerManager.Instance.playerName;
        }

        if (PlayerManager.Instance.highScores.Count > 0)
        {
            currentTopScore = PlayerManager.Instance.highScores[0];
            topScoreText.text = $"Top Score: {currentTopScore.playerName} {currentTopScore.score}";
        }
        else topScoreText.text = "Set A High Score!!!";
        
        ScoreText.text = $"Score : {playerName} {m_Points}";

        OnGameOver += SetGameOverUI;
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {playerName} {m_Points}";
    }

    public void GameOver()
    {
        PlayerManager.Instance.AddHighScore(playerName, m_Points);
        m_GameOver = true;
        GameOverText.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        highScoresButton.gameObject.SetActive(true);
        OnGameOver();
    }

    private void SetGameOverUI()
    {
        mainMenuButton.onClick.AddListener(PlayerManager.Instance.LoadMainMenu);
        quitButton.onClick.AddListener(PlayerManager.Instance.QuitGame);
        highScoresButton.onClick.AddListener(PlayerManager.Instance.LoadHighScoreScreen);
    }
}
