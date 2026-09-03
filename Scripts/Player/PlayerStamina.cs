using UnityEngine;
using StarterAssets;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina = 100f;

    public float staminaDrain = 20f;
    public float staminaRecovery = 15f;

    private UIManager uiManager;
    private StarterAssetsInputs input;

    private bool exhausted = false;
    public float staminaRecoveryThreshold = 50f;

    void Start()
    {
        currentStamina = maxStamina;

        uiManager = FindFirstObjectByType<UIManager>();
        input = GetComponent<StarterAssetsInputs>();

        uiManager.UpdateStamina(currentStamina);
    }

    void Update()
    {
        bool isMoving = input.move != Vector2.zero;

        if (currentStamina <= 0)
        {
            exhausted = true;
        }

        if (exhausted)
        {
            input.sprint = false;
        }

        if (exhausted &&
            currentStamina >= staminaRecoveryThreshold)
        {
            exhausted = false;
        }

        bool canSprint = input.sprint && isMoving && !exhausted;

        if (canSprint)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
        }
        else
        {
            currentStamina += staminaRecovery * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        uiManager.UpdateStamina(currentStamina);

        if (currentStamina <= 0)
        {
            exhausted = true;
            input.sprint = false;
        }

        if (exhausted && currentStamina >= staminaRecoveryThreshold)
        {
            exhausted = false;
        }
    }

    public void IncreaseMaxStamina(int amount)
    {
        maxStamina += amount;
        currentStamina += amount;

        if (currentStamina > maxStamina)
            currentStamina = maxStamina;

        uiManager.UpdateStamina(currentStamina);
    }
}
