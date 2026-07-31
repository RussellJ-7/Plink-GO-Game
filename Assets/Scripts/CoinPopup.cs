using UnityEngine;
using TMPro;

public class CoinPopup : MonoBehaviour
{
    public TextMeshPro popupText;
    public float floatSpeed = 1.5f;
    public float fadeDuration = 0.8f;

    private float timer;
    private Color startColor;

    public void Initialize(int amount, Vector3 position)
    {
        transform.position = position;
        popupText.text = "+" + amount;
        startColor = popupText.color;
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Float upward
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // Fade out
        float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
        popupText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        // Destroy when fully faded
        if (timer >= fadeDuration)
            Destroy(gameObject);
    }
}