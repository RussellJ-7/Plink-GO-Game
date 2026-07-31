using UnityEngine;
using System.Collections;
using TMPro;

public class GoldenPeg : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool isGolden = false;
    private Vector3 originalScale;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        RefreshGoldenState();
    }

    public void RefreshGoldenState()
    {
        int goldenLevel = UpgradeManager.Instance != null
            ? UpgradeManager.Instance.GetLevel("GoldenPeg")
            : 0;

        float goldenChance = goldenLevel * 0.1f;
        isGolden = Random.value < goldenChance;

        if (sr != null)
            sr.color = isGolden ? new Color(1f, 0.84f, 0f) : Color.white;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGolden) return;
        if (!collision.gameObject.CompareTag("Ball")) return;

        int bonus = UpgradeManager.Instance != null
            ? UpgradeManager.Instance.CoinMultiplier * 2
            : 2;

        CurrencyManager.Instance.AddCoins(bonus);
        StartCoroutine(HitAnimation(bonus));
    }

    IEnumerator HitAnimation(int bonus)
    {
        // Scale pop
        float elapsed = 0f;
        float duration = 0.15f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float scale = 1f + Mathf.Sin(t * Mathf.PI) * 0.5f;
            transform.localScale = originalScale * scale;
            yield return null;
        }
        transform.localScale = originalScale;

        // Flash gold -> white -> gold
        sr.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        sr.color = Color.yellow;
        yield return new WaitForSeconds(0.05f);
        sr.color = new Color(1f, 0.84f, 0f);

        // Floating coin text
        SpawnBonusText(bonus);
    }

    void SpawnBonusText(int bonus)
    {
        // Create a world space text object that floats up
        GameObject textObj = new GameObject("GoldenPegBonus");
        textObj.transform.position = transform.position + Vector3.up * 0.3f;

        TextMeshPro tmp = textObj.AddComponent<TextMeshPro>();
        tmp.text = "+" + bonus;
        tmp.fontSize = 3f;
        tmp.color = new Color(1f, 0.84f, 0f);
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;

        // Animate it
        StartCoroutine(FloatAndFade(textObj, tmp));
    }

    IEnumerator FloatAndFade(GameObject obj, TextMeshPro tmp)
    {
        float duration = 0.8f;
        float elapsed = 0f;
        Vector3 startPos = obj.transform.position;
        Color startColor = tmp.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            obj.transform.position = startPos + Vector3.up * t * 1.2f;
            tmp.color = new Color(startColor.r, startColor.g, startColor.b, 1f - t);

            yield return null;
        }

        Destroy(obj);
    }
}