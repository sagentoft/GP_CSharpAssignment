using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradeSelector : MonoBehaviour
{
    [SerializeField] private List<UpgradeData> availableUpgrades;
    [SerializeField] private List<UpgradeData> startingUpgrades;
    [SerializeField] private List<FallbackReward> fallbackRewards;
    [SerializeField] private LevelUpUI levelUpUI;

    private Experience experience;
    private UpgradeManager upgradeManager;
    private int pendingSelections;
    private bool selectionInProgress;

    private void Awake()
    {
        experience = GetComponent<Experience>();
        upgradeManager = GetComponent<UpgradeManager>();
    }

    private void OnEnable()
    {
        experience.OnLevelUp += HandleLevelUp;
    }

    private void OnDisable()
    {
        experience.OnLevelUp -= HandleLevelUp;
    }


    private void Start()
    {
        GenerateStartingChoices();
    }

    
    private void GenerateStartingChoices()
    {
        List<UpgradeData> choices =
            GetRandomChoices(startingUpgrades, 2);

        selectionInProgress = true;

        levelUpUI.ShowChoices(choices, SelectStartingUpgrade, true);
    }
    
    private void SelectStartingUpgrade(UpgradeData upgrade)
    {
        upgradeManager.AcquireUpgrade(upgrade);

        selectionInProgress = false;

        levelUpUI.Hide();
    }

    private void HandleLevelUp()
    {
        pendingSelections++;

        if (!selectionInProgress)
        {
            GenerateChoices();
        }
    }
    
    private void GenerateChoices()
    {
        List<UpgradeData> eligibleUpgrades = new();

        foreach (UpgradeData upgrade in availableUpgrades)
        {
            if (upgradeManager.CanAcquireUpgrade(upgrade))
            {
                eligibleUpgrades.Add(upgrade);
            }
        }

        if (eligibleUpgrades.Count == 0)
        {
            GenerateFallbackChoices();
            return;
        }

        List<UpgradeData> choices =
            GetRandomChoices(eligibleUpgrades, 3);

        selectionInProgress = true;

        levelUpUI.ShowChoices(choices, SelectUpgrade);
    }
    
    private void GenerateFallbackChoices()
    {
        List<FallbackReward> validRewards = fallbackRewards.FindAll(
            reward => reward != null
        );

        if (validRewards.Count == 0)
        {
            Debug.LogWarning("No fallback rewards configured.");
            CompleteSelection();
            return;
        }

        selectionInProgress = true;

        levelUpUI.ShowFallbackChoices(validRewards, SelectFallbackReward);
    }

    private void SelectFallbackReward(FallbackReward reward)
    {
        reward.Apply(gameObject);
        CompleteSelection();
    }
    
    private List<UpgradeData> GetRandomChoices(List<UpgradeData> candidates, int choiceCount)
    {
        List<UpgradeData> availableCandidates = new(candidates);
        List<UpgradeData> choices = new();

        choiceCount = Mathf.Min(choiceCount, availableCandidates.Count);

        for (int i = 0; i < choiceCount; i++)
        {
            int randomIndex = Random.Range(0, availableCandidates.Count);

            choices.Add(availableCandidates[randomIndex]);
            availableCandidates.RemoveAt(randomIndex);
        }

        return choices;
    }

    private void SelectUpgrade(UpgradeData upgrade)
    {
        upgradeManager.AcquireUpgrade(upgrade);
        CompleteSelection();
    }

    private void CompleteSelection()
    {
        pendingSelections--;
        selectionInProgress = false;

        if (pendingSelections > 0)
        {
            GenerateChoices();
        }
        else
        {
            levelUpUI.Hide();
        }
    }
}
