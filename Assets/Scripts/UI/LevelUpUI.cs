using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private UpgradeManager upgradeManager;
    
    private PanelRenderer panelRenderer;

    private VisualElement levelUpPanel;
    private VisualElement choicesContainer;
    
    private Label titleLabel;
    private bool pendingStartingSelection;

    private List<UpgradeData> pendingChoices;
    private Action<UpgradeData> pendingSelectionCallback;
    
    private List<FallbackReward> pendingFallbackChoices;
    private Action<FallbackReward> pendingFallbackCallback;

    private void Awake()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root, int version)
    {
        levelUpPanel = root.Q("level-up-panel");
        choicesContainer = root.Q("choices-container");
        titleLabel = root.Q<Label>("title-label");
        
        levelUpPanel.style.display = DisplayStyle.None;

        if (pendingChoices != null)
        {
            var choices = pendingChoices;
            var callback = pendingSelectionCallback;

            pendingChoices = null;
            pendingSelectionCallback = null;
            
            ShowChoices(choices, callback);
        }

        if (pendingFallbackChoices != null)
        {
            var rewards = pendingFallbackChoices;
            var callback = pendingFallbackCallback;

            pendingFallbackChoices = null;
            pendingFallbackCallback = null;

            ShowFallbackChoices(rewards, callback);
        }
    }

    private void OnDestroy()
    {
        if (panelRenderer != null)
            panelRenderer.UnregisterUIReloadCallback(OnUIReload);
    }

    public void ShowChoices(List<UpgradeData> choices, Action<UpgradeData> onSelected,
        bool isStartingSelection = false)
    {
        if (choicesContainer == null)
        {
            pendingChoices = choices;
            pendingSelectionCallback = onSelected;
            pendingStartingSelection = isStartingSelection;
            return;
        }
        
        titleLabel.text = isStartingSelection
            ? "CHOOSE YOUR WEAPON"
            : "LEVEL UP!";
        
        choicesContainer.Clear();

        foreach (UpgradeData upgrade in choices)
        {
            Button button = new Button();
            button.AddToClassList("upgrade-card");
            
            int nextLevel = upgradeManager.GetNextUpgradeLevel(upgrade);

            UpgradeLevel level = upgrade.GetLevel(nextLevel);

            if (upgrade.Icon != null)
            {
                Image icon = new Image();
                icon.sprite = upgrade.Icon;
                icon.AddToClassList("upgrade-icon");
                button.Add(icon);
            }

            Label nameLabel = new Label(upgrade.UpgradeName);
            nameLabel.AddToClassList("upgrade-name");
            button.Add(nameLabel);
            
            Label levelLabel = new Label($"LEVEL {nextLevel}");
            levelLabel.AddToClassList("upgrade-level");
            button.Add(levelLabel);
            
            Label descriptionLabel = new Label(level.Description);
            descriptionLabel.AddToClassList("upgrade-description");
            button.Add(descriptionLabel);

            button.clicked += () => onSelected(upgrade);
            
            choicesContainer.Add(button);
        }
        
        levelUpPanel.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;
    }
    
    public void ShowFallbackChoices(List<FallbackReward> rewards, Action<FallbackReward> onSelected)
    {
        if (choicesContainer == null)
        {
            pendingFallbackChoices = rewards;
            pendingFallbackCallback = onSelected;
            return;
        }

        choicesContainer.Clear();

        foreach (FallbackReward reward in rewards)
        {
            Button button = new Button();
            button.text = reward.RewardName;

            button.clicked += () => onSelected(reward);

            choicesContainer.Add(button);
        }

        levelUpPanel.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;
    }

    public void Hide()
    {
        levelUpPanel.style.display = DisplayStyle.None;
        Time.timeScale = 1f;
    }
}
