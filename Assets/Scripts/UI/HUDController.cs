using System;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [SerializeField] private Experience playerExperience;

    private PanelRenderer panelRenderer;

    private Label levelLabel;
    private VisualElement xpBarFill;

    private Label timerLabel;
    private Label scoreLabel;

    private GameManager gameManager;

    private void Awake()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        
        gameManager.OnScoreChanged += UpdateScore;
        
        UpdateScore(gameManager.Score);
    }

    private void OnEnable()
    {
        playerExperience.OnExperienceChanged += UpdateExperience;
    }

    private void OnDisable()
    {
        playerExperience.OnExperienceChanged -= UpdateExperience;
        
        if (gameManager != null)
            gameManager.OnScoreChanged -= UpdateScore;
    }

    private void Update()
    {
        if (timerLabel != null)
            UpdateTimer();
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root, int version)
    {
        levelLabel = root.Q<Label>("level-label");
        xpBarFill = root.Q<VisualElement>("xp-bar-fill");
        
        timerLabel = root.Q<Label>("timer-label");
        scoreLabel = root.Q<Label>("score-label");
        scoreLabel.text = $"Score: {GameManager.Instance.Score}";
        
        UpdateExperience(playerExperience.CurrentExperience, playerExperience.ExperienceToNextLevel,
            playerExperience.CurrentLevel);
    }
    
    private void UpdateScore(int newScore)
    {
        if (scoreLabel == null)
            return;
        
        scoreLabel.text = $"Score: {newScore}";
    }
    
    private void UpdateExperience(int currentExperience, int requiredExperience, int currentLevel)
    {
        if (levelLabel == null || xpBarFill == null)
            return;
        
        levelLabel.text = $"Level {currentLevel}";
        
        float progress = requiredExperience > 0 ? (float)currentExperience / requiredExperience : 0f;
        
        xpBarFill.style.width = Length.Percent(Mathf.Clamp01(progress) * 100f);
    }

    private void UpdateTimer()
    {
        float survivalTime = GameManager.Instance.SurvivalTime;
        
        int minutes = Mathf.FloorToInt(survivalTime / 60);
        int seconds = Mathf.FloorToInt(survivalTime % 60);
        
        timerLabel.text = $"{minutes:00}:{seconds:00}";
    }

    private void OnDestroy()
    {
        panelRenderer.UnregisterUIReloadCallback(OnUIReload);
    }
}
