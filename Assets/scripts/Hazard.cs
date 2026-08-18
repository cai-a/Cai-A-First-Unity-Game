using UnityEngine;

public class Hazard : MonoBehaviour
{
    public int damage = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
    
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
   
        }
    }
}
