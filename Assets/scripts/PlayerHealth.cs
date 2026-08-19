using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    public int maxHealth = 3;
    public TMP_Text deathText;
    public TMP_Text healthText;
    public GameManager gameManager;

    void UpdateHealthDisplay()
        {
            healthText.text = "Health: " + health + " / " + maxHealth;
            Debug.Log("Health: " + health);
        }
    public void TakeDamage(int damage)
    {
        if (gameManager.currentState != GameManager.GameState.Playing)
        {
            return;
        }

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthDisplay();

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
    public bool Heal(int amount)
    {
        if (gameManager.currentState != GameManager.GameState.Playing)
        {
            return false;
        }

        if (health >= maxHealth)
        {
            return false;
        }

        health += amount;

        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateHealthDisplay();

        return true;
    }
}