using UnityEngine;

public class Hazard : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
    
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
   
        }
    }
}
