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
    [SerializeField] private float speed = 2f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private float patrolPointRadius = 0.1f;
    [SerializeField] private Transform[] patrolPoints;
    
    private Transform player;
    private GameManager gameManager;
    private Rigidbody2D rb;

    private float damageTimer = 0f;
    private int currentPatrolPoint = 0;
    private bool isInitialized = false;

    public EnemyState CurrentState => currentState;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Enemy is missing a Rigidbody2D.");
            return;
        }
    }

    void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        gameManager = FindFirstObjectByType<GameManager>();

        if (player == null)
        {
            Debug.LogError("Enemy could not find the Player.", this);
            return;
        }

        if (gameManager == null)
        {
            Debug.LogError("Enemy could not find the GameManager.");
            return;
        }
        isInitialized = true;
    }
    
    void Update()    
    {
        if (isInitialized != true)
        {
            return;
        }

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
        if (isInitialized != true)
        {
            return;
        }
        
        if (gameManager.currentState != GameManager.GameState.Playing)
        {
            return;
        }

        Vector2 toPlayer = (Vector2)player.position - rb.position;

        float distanceToPlayer = toPlayer.magnitude;



        if (distanceToPlayer > chaseRange)
        {
            ChangeState(EnemyState.Idle);
        }
        else if (distanceToPlayer > attackRange)
        {
            ChangeState(EnemyState.Chase);
        }
        else
        {
            ChangeState(EnemyState.Attack);
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

    void MoveToward(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - rb.position).normalized;

        rb.MovePosition(
            rb.position + direction * speed * Time.fixedDeltaTime
        );
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

    void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (isInitialized != true)
        {
            return;
        }

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

