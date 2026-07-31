using UnityEngine;

public class PegBoard : MonoBehaviour
{
    [Header("Peg Board Settings")]
    public int rows = 8;
    public int columns = 8;
    public float horizontalSpacing = 1.0f;
    public float verticalSpacing = 1.0f;
    public float rowOffset = 0.5f;
    public GameObject PegPrefab;

    void Start()
    {
        GenerateBoard();
    }

    void GenerateBoard()
    {
        for (int row = 0; row < rows; row++)
        {
            float xOffset = (row % 2 == 0) ? 0f : rowOffset;
            int pegsInRow = columns;

            for (int col = 0; col < pegsInRow; col++)
            {
                float xPos = (col * horizontalSpacing) - ((pegsInRow - 1) * horizontalSpacing / 2f) + xOffset;
                float yPos = -(row * verticalSpacing);

                Vector3 spawnPos = new Vector3(xPos, yPos, 0);
                Instantiate(PegPrefab, spawnPos, Quaternion.identity, transform);
            }
        }
    }
}
