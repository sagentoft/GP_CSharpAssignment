using System;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] private int currentExperience = 0;
    [SerializeField] private int experienceToNextLevel = 10;
    [SerializeField] private int baseExperience = 10;
    [SerializeField] private int experienceIncreasePerLevel = 5;
    [SerializeField] private int currentLevel = 1;
    
    public void AddExperience(int amount)
    {
        currentExperience += amount;

        while (currentExperience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentExperience -= experienceToNextLevel;
        currentLevel++;
        
        experienceToNextLevel = CalculateExperienceToNextLevel();
    }

    private int CalculateExperienceToNextLevel()
    {
        return baseExperience + ((currentLevel - 1) * experienceIncreasePerLevel);
    }
}
