using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public GameObject panel;

    public Button button1;
    public Button button2;
    public Button button3;

    public TMP_Text button1Text;
    public TMP_Text button2Text;
    public TMP_Text button3Text;

    public TMP_Text button1Description;
    public TMP_Text button2Description;
    public TMP_Text button3Description;

    public Upgrade[] upgrades;

    private Upgrade[] currentChoices;

    private WeaponShoot weapon;

    private PlayerHealth playerHealth;

    private PlayerStamina playerStamina;

    private ZombieSpawner spawner;

    void Start()
    {
        panel.SetActive(false);

        weapon = FindFirstObjectByType<WeaponShoot>();

        playerHealth = FindFirstObjectByType<PlayerHealth>();

        playerStamina = FindFirstObjectByType<PlayerStamina>();
    }

    public void ShowUpgradePanel(ZombieSpawner zombieSpawner)
    {
        spawner = zombieSpawner;

        Time.timeScale = 0;

        panel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GenerateChoices();
    }

    void GenerateChoices()
    {
        List<Upgrade> pool =
            new List<Upgrade>(upgrades);

        currentChoices = new Upgrade[3];

        for (int i = 0; i < 3; i++)
        {
            int random =
                Random.Range(0, pool.Count);

            currentChoices[i] = pool[random];

            pool.RemoveAt(random);
        }

        button1Text.text = currentChoices[0].title;
        button1Description.text = currentChoices[0].description;

        button2Text.text = currentChoices[1].title;
        button2Description.text = currentChoices[1].description;

        button3Text.text = currentChoices[2].title;
        button3Description.text = currentChoices[2].description;
    }

    public void ChooseUpgrade(int index)
    {
        ApplyUpgrade(currentChoices[index]);

        GameMetricsManager.Instance.RegisterUpgrade(currentChoices[index].title);

        panel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;

        spawner.StartNextWave();
    }

    void ApplyUpgrade(Upgrade upgrade)
    {
        switch (upgrade.type)
        {
            case UpgradeType.MaxHealth:

                playerHealth.IncreaseMaxHealth((int)upgrade.value);

                break;

            case UpgradeType.Damage:

                weapon.IncreaseDamage((int)upgrade.value);

                break;

            case UpgradeType.MaxStamina:

                playerStamina.IncreaseMaxStamina((int)upgrade.value);

                break;

            case UpgradeType.AttackRange:

                weapon.IncreaseAttackRange(upgrade.value);

                break;

            case UpgradeType.ReloadSpeed:

                weapon.IncreaseReloadSpeed(upgrade.value);

                break;

            case UpgradeType.FireRate:

                weapon.IncreaseFireRate(upgrade.value);

                break;

            case UpgradeType.MagazineSize:

                weapon.IncreaseMagazineSize((int)upgrade.value);

                break;
        }
    }
}
