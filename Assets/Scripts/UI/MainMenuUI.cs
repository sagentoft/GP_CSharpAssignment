using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private PanelRenderer panelRenderer;

    private Button playButton;
    private Button quitButton;
    private Label highscoreLabel;

    private void Awake()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root, int version)
    {
        if (playButton != null)
            playButton.clicked -= PlayGame;

        if (quitButton != null)
            quitButton.clicked -= QuitGame;
        
        playButton = root.Q<Button>("play-button");
        quitButton = root.Q<Button>("quit-button");
        highscoreLabel = root.Q<Label>("highscore-label");
        
        playButton.clicked += PlayGame;
        quitButton.clicked += QuitGame;

        highscoreLabel.text = $"High score: {SaveManager.HighScore:N0}";
    }

    private void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        
        if (playButton != null)
            playButton.clicked -= PlayGame;
        
        if (quitButton != null)
            quitButton.clicked -= QuitGame;
    }
}
