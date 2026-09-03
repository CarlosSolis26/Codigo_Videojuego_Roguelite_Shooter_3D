using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    private NavMeshAgent agent;
    private Animator animator;

    private EnemyHealth health;

    public float attackDistance = 0.8f;
    public int damage = 25;
    public float attackCooldown = 1f;
    public float baseSpeed;

    private float nextAttackTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = GetComponent<EnemyHealth>();

        if (agent != null)
        {
            baseSpeed = agent.speed;
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    public void SetSpeed(float newSpeed)
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (agent != null)
        {
            agent.speed = newSpeed;
        }
    }

    void Update()
    {
        if (health != null && health.IsDead)
        {
            return;
        }

        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackDistance)
        {
            agent.SetDestination(player.position);

            float speed = agent.velocity.magnitude;

            animator.SetFloat("Speed", speed);
        }
        else
        {
            agent.SetDestination(transform.position);

            animator.SetFloat("Speed", 0);

            Attack();
        }
    }

    void Attack()
    {
        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + attackCooldown;

        animator.SetTrigger("Attack");
    }

    public void DealDamage()
    {
        if (player == null) return;

        float distance = Vector3.Distance(
            transform.position,
            player.position);

        if (distance <= attackDistance + 0.3f)
        {
            PlayerHealth playerHealth =
                player.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
