using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Coin Display")]
    public TextMeshProUGUI coinsText;

    void Start()
    {
        if (CurrencyManager.Instance == null)
        {
            Debug.LogError("CurrencyManager not found in scene!");
            return;
        }

        // Subscribe to coin changes
        CurrencyManager.Instance.OnCoinsChanged += UpdateCoinsDisplay;

        // Set initial display
        UpdateCoinsDisplay();
    }

    void OnDisable()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCoinsChanged -= UpdateCoinsDisplay;
    }

    void UpdateCoinsDisplay()
    {
        coinsText.text = CurrencyManager.Instance.GetFormattedCoins();
    }

}
