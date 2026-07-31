using UnityEngine;

public class ResetBall : MonoBehaviour
{
    private Rigidbody2D rb;
    private int coinsEarned = 0;
    
    private float stillTimer = 0f; 
    private float timeLimit = .5f;  

    [Header("Spawn Settings")]
    public GameObject ballPrefab; // Drag your Ball Prefab here in the Inspector


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Ensure the Rigidbody never goes to sleep
        if (rb != null)
        {
            rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        }
    }

    void Update()
    {
        if (rb != null)
        {
            if (rb.linearVelocity.magnitude <= 0.1f)
            {
                stillTimer += Time.deltaTime;

                if (stillTimer >= timeLimit)
                {
                    ResetLogic();
                }
            }
            else
            {
                stillTimer = 0f;
            }
        }
    }

    private void ResetLogic()
    {
        // 2. Log data before destruction
        coinsEarned++;
        Debug.Log("Ball reset. Coins earned: " + coinsEarned);

        // 3. Destroy this old object last
        Destroy(gameObject);
    }
}