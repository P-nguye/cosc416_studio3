using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if Player touches the coin
        {
            if (GameManager.instance != null) // Ensure GameManager exists
            {
                GameManager.instance.AddScore(1); // Increase score
                Destroy(gameObject); // Remove the collected coin
            }
            else
            {
                Debug.LogError("GameManager.instance is null! Make sure it's in the scene.");
            }
        }
    }
}
