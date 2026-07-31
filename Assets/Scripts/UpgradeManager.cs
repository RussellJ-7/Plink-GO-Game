using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("Upgrades")]
    public UpgradeData betterBalls;
    public UpgradeData fasterDrop;
    public UpgradeData extraBall;
    public UpgradeData coinMagnet;
    public UpgradeData goldenPeg;
    public UpgradeData autoDropper;

    // Track current level of each upgrade
    private Dictionary<string, int> upgradeLevels = new Dictionary<string, int>();

    // Public properties other scripts can read
    public int CoinMultiplier => 1 + GetLevel("BetterBalls");
    public float DropCooldown => Mathf.Max(0.05f, 0.5f / (1f + GetLevel("FasterDrop") * 0.5f));
    public int BallsPerDrop => 1 + GetLevel("ExtraBalls");
    public float SlotBonusMultiplier => 1f + (GetLevel("CoinMagnet") * 0.5f);
    public bool AutoDropEnabled => GetLevel("AutoDropper") > 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int GetLevel(string upgradeName)
    {
        return upgradeLevels.ContainsKey(upgradeName) ? upgradeLevels[upgradeName] : 0;
    }

    public bool PurchaseUpgrade(UpgradeData upgrade)
    {
        int currentLevel = GetLevel(upgrade.upgradeName);

        if (currentLevel >= upgrade.maxLevel) return false;

        double cost = upgrade.GetCostForLevel(currentLevel);

        if (CurrencyManager.Instance.SpendCoins(cost))
        {
            if (!upgradeLevels.ContainsKey(upgrade.upgradeName))
                upgradeLevels[upgrade.upgradeName] = 0;

            upgradeLevels[upgrade.upgradeName]++;

            // Refresh golden pegs if that upgrade was purchased
            if (upgrade.upgradeName == "GoldenPeg")
                RefreshGoldenPegs();

            return true;
        }

        return false;
    }


    public double GetUpgradeCost(UpgradeData upgrade)
    {
        return upgrade.GetCostForLevel(GetLevel(upgrade.upgradeName));
    }

    void RefreshGoldenPegs()
    {
        GoldenPeg[] allPegs = FindObjectsByType<GoldenPeg>(FindObjectsSortMode.None);
        foreach (GoldenPeg peg in allPegs)
            peg.RefreshGoldenState();
    }
}