using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {

            bool healed = playerHealth.Heal(healAmount);  
            
            if (healed)
            {
                Destroy(gameObject);
            }
        }
    }
}
