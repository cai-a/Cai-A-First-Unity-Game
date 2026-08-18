using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    public int maxHealth = 3;
    public TMP_Text deathText;
    public TMP_Text healthText;
    public GameManager gameManager;
    public void TakeDamage(int damage)
    {
        if (gameManager.currentState != GameManager.GameState.Playing)
        {
            return;
        }

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        healthText.text = "Health: " + health;

        Debug.Log("Health: " + health);

        if (health <= 0) 
        {
            gameManager.LoseGame();

            PlayerMovement playerMovement = GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }
            Debug.Log("Player Died!");
            deathText.gameObject.SetActive(true);
        }
    }
}