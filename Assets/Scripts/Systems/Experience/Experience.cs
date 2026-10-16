using System;
using UnityEngine;

public class Experience : MonoBehaviour
{
    
    [SerializeField] private int currentExperience = 0;
    [SerializeField] private int experienceToNextLevel = 10;
    [SerializeField] private int baseExperience = 10;
    [SerializeField] private int experienceIncreasePerLevel = 3;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private AudioClip levelUpSound;
    
    public int CurrentExperience => currentExperience;
    public int ExperienceToNextLevel => experienceToNextLevel;
    public int CurrentLevel => currentLevel;
    
    public event Action OnLevelUp;
    public event Action<int, int, int> OnExperienceChanged;
    
    public void AddExperience(int amount)
    {
        currentExperience += amount;

        while (currentExperience >= experienceToNextLevel)
        {
            LevelUp();
        }
        
        OnExperienceChanged?.Invoke(currentExperience, experienceToNextLevel, CurrentLevel);
    }

    private void LevelUp()
    {
        currentExperience -= experienceToNextLevel;
        currentLevel++;
        
        experienceToNextLevel = CalculateExperienceToNextLevel();
        
        AudioManager.Instance.PlaySFX(levelUpSound, 0.7f);
        
        OnLevelUp?.Invoke();
    }

    private int CalculateExperienceToNextLevel()
    {
        return baseExperience + ((currentLevel - 1) * experienceIncreasePerLevel);
    }
}
