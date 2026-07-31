using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    [Header("Upgrade")]
    public UpgradeData upgrade;

    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI levelText;
    public Button button;

    void Start()
    {
        button.onClick.AddListener(OnClickUpgrade);
        CurrencyManager.Instance.OnCoinsChanged += RefreshUI;
        RefreshUI();
    }

    void OnDisable()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCoinsChanged -= RefreshUI;
    }

    void OnClickUpgrade()
    {
        if (UpgradeManager.Instance.PurchaseUpgrade(upgrade))
            RefreshUI();
    }

    void RefreshUI()
    {
        int level = UpgradeManager.Instance.GetLevel(upgrade.upgradeName);
        double cost = UpgradeManager.Instance.GetUpgradeCost(upgrade);

        nameText.text = upgrade.upgradeName;
        levelText.text = "Lv. " + level + " / " + upgrade.maxLevel;
        costText.text = CurrencyManager.Instance.GetFormattedCoins();

        // Grey out button if cant afford or max level
        bool canAfford = CurrencyManager.Instance.coins >= cost;
        bool maxLevel = level >= upgrade.maxLevel;
        button.interactable = canAfford && !maxLevel;

        if (maxLevel)
            costText.text = "MAX";
    }
}