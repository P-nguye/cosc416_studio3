using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the correct tag
        {
            if (GameManager.instance != null) // Prevent null errors
            {
                GameManager.instance.AddScore(1); // Increase score
                Destroy(gameObject); // Destroy the collected coin
            }
            else
            {
                Debug.LogError("GameManager.instance is null! Make sure GameManager is in the scene.");
            }
        }
    }
}
