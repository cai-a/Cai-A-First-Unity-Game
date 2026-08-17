using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    public bool isDead = false;

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }
        
        health -= damage;

        Debug.Log("Health: " + health);

        if (health <= 0) 
        {
            isDead = true;
            Debug.Log("Player Died!");
        }
    }
}