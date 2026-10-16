using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{
    private PanelRenderer panelRenderer;

    private VisualElement root;
    private Button restartButton;
    private Button mainMenuButton;
    
    private Label finalScoreLabel;
    private Label survivalTimeLabel;

    private void Awake()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root, int version)
    {
        if (restartButton != null)
            restartButton.clicked -= RestartGame;

        if (mainMenuButton != null)
            mainMenuButton.clicked -= LoadMainMenu;
        
        this.root = root;
        
        restartButton = root.Q<Button>("restart-button");
        restartButton.clicked += RestartGame;
        
        mainMenuButton = root.Q<Button>("main-menu-button");
        mainMenuButton.clicked += LoadMainMenu;
        
        finalScoreLabel = root.Q<Label>("final-score");
        survivalTimeLabel = root.Q<Label>("survival-time");

        Hide();
    }
    
    private void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Show()
    {
        
        GameManager gameManager = GameManager.Instance;

        finalScoreLabel.text = gameManager.Score.ToString("N0");

        int totalSeconds = Mathf.FloorToInt(gameManager.SurvivalTime);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        survivalTimeLabel.text = $"{minutes:00}:{seconds:00}";

        root.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        root.style.display = DisplayStyle.None;
    }

    private void OnDestroy()
    {
        panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        
        if (restartButton != null)
            restartButton.clicked -= RestartGame;
        
        if (mainMenuButton != null)
            mainMenuButton.clicked -= LoadMainMenu;
    }
}
