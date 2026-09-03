using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Slider healthBar;
    public Slider staminaBar;
    public TMP_Text ammoText;

    public void UpdateHealth(int health)
    {
        healthBar.value = health;
    }

    public void UpdateStamina(float stamina)
    {
        staminaBar.value = stamina;
    }

    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        ammoText.text = "Ammo: " + currentAmmo + " / " + maxAmmo;
    }
}
