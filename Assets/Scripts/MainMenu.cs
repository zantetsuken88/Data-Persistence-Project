using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button quitButton;
    [SerializeField] private Button highScoreButton;
    [SerializeField] private Button newGameButton;

    private void Start()
    {
        newGameButton.onClick.AddListener(delegate { PlayerManager.Instance.NewGame(); });
        quitButton.onClick.AddListener(delegate { PlayerManager.Instance.QuitGame(); });
        highScoreButton.onClick.AddListener(delegate { PlayerManager.Instance.LoadHighScoreScreen(); });
    }
}