using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack
    }

    public EnemyState currentState = EnemyState.Idle;
    
    public Transform player;
    public GameManager gameManager;
    public float speed = 2f;
    public float attackRange = 1f;
    public float chaseRange = 5f;
    public int damage = 1;
    public float damageCooldown = 1f;
    private float damageTimer = 0f;
    private Rigidbody2D rb;
    public Transform[] patrolPoints;
    private int currentPatrolPoint = 0;

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

    void Patrol()
    {
        if (patrolPoints.Length < 2)
        {
            return;
        }
        
        Transform targetPoint = patrolPoints[currentPatrolPoint];

        Vector2 toTarget =
            (Vector2)targetPoint.position - rb.position;

        float distanceToTarget = toTarget.magnitude;

        if (distanceToTarget < 0.1f)
        {
            currentPatrolPoint =
                (currentPatrolPoint + 1) % patrolPoints.Length;
            return;
        }

         Vector2 direction = toTarget.normalized;
         rb.MovePosition(
            rb.position + direction * speed * Time.fixedDeltaTime
        );
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
            currentState = EnemyState.Idle;
        }
        else if (distanceToPlayer > attackRange)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.Attack;
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                Patrol();
                break;

            case EnemyState.Chase:
                Vector2 direction = toPlayer.normalized;

                rb.MovePosition(
                    rb.position + direction * speed * Time.fixedDeltaTime
                );
                break;

            case EnemyState.Attack:
                break;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (gameManager.currentState != GameManager.GameState.Playing)
        {
            return;
        }

        if (currentState != EnemyState.Attack)
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

