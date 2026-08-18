using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    public bool isDead = false;
    public TMP_Text deathText;
    public GameManager gameManager;

    void Update()
    {
        if (gameManager.currentState == GameManager.GameState.Dead && Keyboard.current.rKey.wasPressedThisFrame)
        {
            gameManager.RestartGame();
        }
    }

    public void TakeDamage(int damage)
    {
        if (gameManager.currentState != GameManager.GameState.Playing)
        {
            return;
        }

        if (isDead)
        {
            return;
        }

        health -= damage;

        Debug.Log("Health: " + health);

        if (health <= 0) 
        {
            gameManager.LoseGame();

            isDead = true;

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