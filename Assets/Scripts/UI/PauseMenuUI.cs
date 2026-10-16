using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private PanelRenderer panelRenderer;
    private VisualElement root;

    private Button resumeButton;
    private Button restartButton;
    private Button mainMenuButton;

    private bool isPaused;

    private void Awake()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root, int version)
    {
        UnregisterButtons();

        this.root = root;

        resumeButton = root.Q<Button>("resume-button");
        restartButton = root.Q<Button>("restart-button");
        mainMenuButton = root.Q<Button>("main-menu-button");

        resumeButton.clicked += Resume;
        restartButton.clicked += Restart;
        mainMenuButton.clicked += MainMenu;

        root.style.display = isPaused
            ? DisplayStyle.Flex
            : DisplayStyle.None;
    }

    private void Pause()
    {
        isPaused = true;
        root.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;
    }

    private void Resume()
    {
        isPaused = false;
        root.style.display = DisplayStyle.None;
        Time.timeScale = 1f;
    }

    private void Restart()
    {
        Time.timeScale = 1f;
        GameManager.Instance.RestartGame();
    }

    private void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    public void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else if (Time.timeScale > 0f)
        {
            Pause();
        }
    }

    private void UnregisterButtons()
    {
        if (resumeButton != null)
            resumeButton.clicked -= Resume;

        if (restartButton != null)
            restartButton.clicked -= Restart;

        if (mainMenuButton != null)
            mainMenuButton.clicked -= MainMenu;
    }

    private void OnDestroy()
    {
        UnregisterButtons();

        if (panelRenderer != null)
            panelRenderer.UnregisterUIReloadCallback(OnUIReload);
    }
    
    
}