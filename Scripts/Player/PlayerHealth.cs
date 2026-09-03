using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    private Animator animator;
    private UIManager uiManager;

    public int maxHealth = 100;

    private int currentHealth;
    private bool isDead = false;

    private GameOverManager gameOverManager;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();

        uiManager = FindFirstObjectByType<UIManager>();

        uiManager.UpdateHealth(currentHealth);

        gameOverManager = FindFirstObjectByType<GameOverManager>();
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        uiManager.UpdateHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        GameMetricsManager.Instance.RegisterDamage(damage);

        animator.SetTrigger("Hit");

        Debug.Log("Vida jugador: " + currentHealth);

        uiManager.UpdateHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        animator.SetTrigger("Death");

        Debug.Log("GAME OVER");

        GetComponent<PlayerInput>().enabled = false;
        GetComponent<WeaponShoot>().enabled = false;
        GetComponent<PlayerAimCamera>().enabled = false;
        GetComponent<PlayerAimRotation>().enabled = false;

        GameMetricsManager.Instance.FinishGame();

        Invoke(nameof(ShowGameOverScreen), 2f);
    }

    void ShowGameOverScreen()
    {
        gameOverManager.ShowGameOver();
    }
}
