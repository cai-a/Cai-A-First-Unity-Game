using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack
    }

    [SerializeField] private EnemyState currentState = EnemyState.Idle;
    
    private Transform player;
    private GameManager gameManager;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private float patrolPointRadius = 0.1f;
    [SerializeField] private Transform[] patrolPoints;
    private float damageTimer = 0f;
    private int currentPatrolPoint = 0;
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

    void MoveToward(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - rb.position).normalized;

        rb.MovePosition(
            rb.position + direction * speed * Time.fixedDeltaTime
        );
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

        if (distanceToTarget < patrolPointRadius)
        {
            currentPatrolPoint =
                (currentPatrolPoint + 1) % patrolPoints.Length;
            return;
        }

        MoveToward((Vector2)targetPoint.position);
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
                MoveToward((Vector2)player.position);
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
    void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(
                transform.position,
                chaseRange
            );

            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(
                transform.position,
                attackRange
            );
        }
}

