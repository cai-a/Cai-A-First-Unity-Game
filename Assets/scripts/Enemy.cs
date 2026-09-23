using System.Net;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack
    }

    [Header("State")]
    [SerializeField] private EnemyState currentState = EnemyState.Idle;

    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float chaseRange = 5f;

    [Header("Combat")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float damageCooldown = 1f;

    [Header("Patrol")]
    [SerializeField] private float patrolPointRadius = 0.1f;
    [SerializeField] private Transform[] patrolPoints;
    
    private Transform player;
    private GameManager gameManager;
    private Rigidbody2D rb;

    private float damageTimer = 0f;
    private int currentPatrolPoint = 0;
    private bool isInitialized = false;
    private static int enemyCount = 0;
    public static int EnemyCount => enemyCount;

    public EnemyState CurrentState => currentState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        enemyCount++;
        Debug.Log(
        gameObject.name +
        " created. Total enemies: " +
        enemyCount
        );
    }

    private void Start()
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
        Debug.Log("I am " + gameObject.name);


    }


    private void Update()    
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

    private void FixedUpdate()
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

    private int RepairWithCoin(
        int coins,
        Toolbox toolbox,
        string toolName,
        IRepairable target,
        int cost)
    {
        if (cost <= 0)
        {
            return coins;
        }
            
        if (coins >= cost && toolbox.UseTool(toolName, target))
        {
            coins -= cost;
        }

            return coins;


    }

    private void UseNamedTool(
        Dictionary<string, RepairTool> toolbox,
        string toolName,
        IRepairable target)
    {
        if (toolbox.TryGetValue(toolName, out RepairTool namedTool))
        {
            namedTool.Use(target);
        }
        else
        {
            Debug.Log("Tool not found: " + toolName);
        }
        
    }



    private List<RepairTool> CreateTools(int count, int repairAmount)
    {
        List<RepairTool> newTools = new List<RepairTool>();

        for (int i = 0; i < count; i++)
        {
            newTools.Add(new RepairTool(repairAmount));
        }

        return newTools;
    }



    private void RemoveEmptyTools(List<RepairTool> tools)
    {
        for (int i = tools.Count - 1; i >= 0; i--)
        {
            if (tools[i].UsesRemaining == 0)
            {
                tools.RemoveAt(i);
            }
        }
    }
    private void ReplaceTools(List<RepairTool> tools)
    {
        tools = new List<RepairTool>();

        tools.Add(new RepairTool(3));

        Debug.Log("Inside method: " + tools.Count);
    }



    private void DealDamage(IDamageable target, int amount)
    {
        target.TakeDamage(amount);
    }
    
    private void OnDestroy()
    {
        enemyCount--;

        Debug.Log(
            gameObject.name +
            " destroyed. Total enemies: " +
            enemyCount
        );
    }

    private void MoveToward(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - rb.position).normalized;

        rb.MovePosition(
            rb.position + direction * speed * Time.fixedDeltaTime
        );
    }

    private void Patrol()
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

    private void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
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

    private void OnDrawGizmosSelected()
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

