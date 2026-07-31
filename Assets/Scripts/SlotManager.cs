using UnityEngine;

public class SlotManager : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject slotPrefab;

    [Header("Layout")]
    public int slotCount = 7;
    public float slotSpacing = 1.0f;
    public float yPosition = -8.5f;

    [Header("Multipliers")]
    public int[] multipliers = { 1, 2, 5, 10, 5, 2, 1 };

    void Start()
    {
        GenerateSlots();
    }

    void GenerateSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            float xPos = (i * slotSpacing) - ((slotCount - 1) * slotSpacing / 2f);
            Vector3 spawnPos = new Vector3(xPos, yPosition, 0);

            GameObject slot = Instantiate(slotPrefab, spawnPos, Quaternion.identity, transform);

            SlotScorer scorer = slot.GetComponent<SlotScorer>();
            if (scorer != null)
            {
                int multiplier = (i < multipliers.Length) ? multipliers[i] : 1;
                scorer.coinMultiplier = multiplier;
                scorer.Initialize();
            }
        }
    }
}