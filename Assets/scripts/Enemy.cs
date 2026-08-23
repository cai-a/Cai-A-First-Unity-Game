using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public GameManager gameManager;
    public float speed = 2f;
    public int damage = 1;
    public float damageCooldown = 1f;
    private float damageTimer = 0f;

    void Update()
    {
        if (gameManager.currentState != GameManager.GameState.Playing)
        {
            return;
        }

        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
        }

        Vector3 direction = player.position - transform.position;

        direction = direction.normalized;

        transform.position += direction * speed * Time.deltaTime;

        Debug.Log("Timer: " + damageTimer);
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (gameManager.currentState != GameManager.GameState.Playing)
        {
            return;
        }

        if (damageTimer > 0f)
        {
            return;
        }

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            damageTimer = damageCooldown;
        }
        
    }
}

