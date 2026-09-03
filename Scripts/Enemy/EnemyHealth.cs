using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private Animator animator;
    private bool isDead = false;

    public ZombieSpawner spawner;

    public bool IsDead
    {
        get { return isDead; }
    }

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void SetHealth(int newHealth)
    {
        maxHealth = newHealth;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        Debug.Log("Zombie recibió " + damage + " de daño");

        if (currentHealth > 0)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        GameMetricsManager.Instance.RegisterKill();

        isDead = true;

        GetComponent<EnemyAI>().enabled = false;

        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.enabled = false;
        }

        if (spawner != null)
        {
            spawner.ZombieKilled();
        }

        animator.SetTrigger("Death");

        Destroy(gameObject, 5f);
    }
}
