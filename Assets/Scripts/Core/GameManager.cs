using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameOverUI gameOverUI;
    public static GameManager Instance { get; private set; }
    
    public bool IsGameOver { get; private set; }
    public float SurvivalTime { get; private set; }
    public int Score { get; private set; }

    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (IsGameOver)
            return;
        SurvivalTime += Time.deltaTime;
    }

    public void AddScore(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }

    public void GameOver()
    {
        if (IsGameOver)
            return;
        
        IsGameOver = true;
        SaveManager.TrySaveHighScore(Score);
        Time.timeScale = 0f;

        gameOverUI.Show();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
