using UnityEngine;
using TMPro;

public class SlotScorer : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject coinPopupPrefab;

    [Header("Settings")]
    public int coinMultiplier = 1;
    public TextMeshProUGUI label;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (label != null)
            label.text = coinMultiplier + "x";

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (coinMultiplier >= 10)
                sr.color = new Color(1f, 0.84f, 0f);
            else if (coinMultiplier >= 5)
                sr.color = new Color(0.5f, 0.9f, 0.5f);
            else if (coinMultiplier >= 2)
                sr.color = new Color(0.5f, 0.7f, 1f);
            else
                sr.color = new Color(0.8f, 0.8f, 0.8f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;

        Ball ball = other.GetComponent<Ball>();
        if (ball == null || ball.hasScored) return;

        ball.hasScored = true;

        int baseCoins = coinMultiplier;
        int betterBallsMultiplier = UpgradeManager.Instance != null 
            ? UpgradeManager.Instance.CoinMultiplier 
            : 1;
        float magnetMultiplier = UpgradeManager.Instance != null 
            ? UpgradeManager.Instance.SlotBonusMultiplier 
            : 1f;

        int totalCoins = (int)(baseCoins * betterBallsMultiplier * magnetMultiplier);

        CurrencyManager.Instance.AddCoins(totalCoins);

        if (coinPopupPrefab != null)
        {
            GameObject popup = Instantiate(coinPopupPrefab, transform.position, Quaternion.identity);
            popup.GetComponent<CoinPopup>().Initialize(totalCoins, transform.position);
        }

        Destroy(other.gameObject);
    }


    
}



