using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Plinko/Upgrade")]
public class UpgradeData : ScriptableObject
{
    [Header("Info")]
    public string upgradeName;
    public string description;

    [Header("Costs")]
    public double baseCost;
    public float costMultiplier = 1.8f; // Cost increases by this each level

    [Header("Settings")]
    public int maxLevel = 10;
    public float effectPerLevel = 1f; // How much the effect increases per level

    public double GetCostForLevel(int level)
    {
        return System.Math.Floor(baseCost * System.Math.Pow(costMultiplier, level));
    }
}
