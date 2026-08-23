using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public GameManager gameManager;
    public float speed = 2f;
    public float chaseRange = 5f;
    public int damage = 1;
    public float damageCooldown = 1f;
    private float damageTimer = 0f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;

        }

        gameManager = FindFirstObjectByType<GameManager>();
    }
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
    }

    void FixedUpdate()
    {
        if (gameManager.currentState != GameManager.GameState.Playing)
        {
            return;
        }

        Vector2 toPlayer = (Vector2)player.position - rb.position;

        float distanceToPlayer = toPlayer.magnitude;



        if (distanceToPlayer > chaseRange)
        {
            return;
        }

        Vector2 direction = toPlayer.normalized;
    

            rb.MovePosition(
            rb.position + direction * speed * Time.fixedDeltaTime
        );
    
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

