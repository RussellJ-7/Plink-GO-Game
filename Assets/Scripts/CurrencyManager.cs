using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [Header("Currency")]
    public double coins = 0;

    public event Action OnCoinsChanged;

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

    public void AddCoins(double amount)
    {
        coins += amount;
        OnCoinsChanged?.Invoke();
    }

    public bool SpendCoins(double amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            OnCoinsChanged?.Invoke();
            return true;
        }

        return false;
    }

    public string GetFormattedCoins()
    {
        if (coins >= 1_000_000_000)
            return ((long)(coins / 1_000_000_000)).ToString() + "." + 
                ((long)(coins / 100_000_000) % 10).ToString() + "B";
        if (coins >= 1_000_000)
            return ((long)(coins / 1_000_000)).ToString() + "." + 
                ((long)(coins / 100_000) % 10).ToString() + "M";
        if (coins >= 1_000)
            return ((long)(coins / 1_000)).ToString() + "." + 
                ((long)(coins / 100) % 10).ToString() + "K";

        return ((long)coins).ToString();
    }
}
