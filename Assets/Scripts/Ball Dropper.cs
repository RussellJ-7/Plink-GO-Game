using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BallDropper : MonoBehaviour
{
    [Header("Settings")]
    public GameObject ballPrefab;
    public float dropHeight = 5f;

    private float cooldownTimer = 0f;
    private float autoDropperTimer = 0f;

    void Update()
    {
        float cooldown = UpgradeManager.Instance != null
            ? UpgradeManager.Instance.DropCooldown
            : 0.5f;

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            if (touch.press.wasPressedThisFrame && cooldownTimer <= 0f)
            {
                Vector2 touchPos = touch.position.ReadValue();
                if (!IsPointerOverUI(touchPos))
                {
                    DropBalls(touchPos);
                    cooldownTimer = cooldown;
                }
            }
        }
        else if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame && cooldownTimer <= 0f)
            {
                Vector2 mousePos = Mouse.current.position.ReadValue();
                if (!IsPointerOverUI(mousePos))
                {
                    DropBalls(mousePos);
                    cooldownTimer = cooldown;
                }
            }
        }

        if (UpgradeManager.Instance != null && UpgradeManager.Instance.AutoDropEnabled)
        {
            autoDropperTimer -= Time.deltaTime;
            if (autoDropperTimer <= 0f)
            {
                float randomX = Random.Range(-2.0f, 2.0f);
                Vector2 randomScreenPos = Camera.main.WorldToScreenPoint(
                    new Vector3(randomX, dropHeight, 0));
                DropBalls(randomScreenPos);
                autoDropperTimer = cooldown * 2f;
            }
        }
    }

    void DropBalls(Vector2 screenPos)
    {
        int ballCount = UpgradeManager.Instance != null
            ? UpgradeManager.Instance.BallsPerDrop
            : 1;

        Debug.Log("DropBalls called. ballCount: " + ballCount + 
                " | ExtraBall level: " + UpgradeManager.Instance?.GetLevel("ExtraBall"));

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, 0));

        if (worldPos.x < -3.5f || worldPos.x > 3.5f)
            return;

        for (int i = 0; i < ballCount; i++)
        {
            float spread = 0.6f;
            float offsetX = (ballCount == 1) ? 0 :
                Mathf.Lerp(-spread, spread, (float)i / (ballCount - 1));

            float clampedX = Mathf.Clamp(worldPos.x + offsetX, -3.5f, 3.5f);
            Vector3 spawnPos = new Vector3(clampedX, dropHeight, 0);
            Debug.Log("Spawning ball " + i + " at X: " + clampedX);
            Instantiate(ballPrefab, spawnPos, Quaternion.identity);
        }
    }

    bool IsPointerOverUI(Vector2 screenPos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = screenPos;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }
}
