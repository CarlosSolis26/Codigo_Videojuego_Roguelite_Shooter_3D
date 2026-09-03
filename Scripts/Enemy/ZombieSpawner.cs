using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    private UpgradeManager upgradeManager;

    [Header("Zombie Prefabs")]
    public GameObject walkerZombie;
    public GameObject runnerZombie;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("UI")]
    public TMP_Text waveText;
    public TMP_Text waveMessage;

    [Header("Wave Settings")]
    public int currentWave = 1;

    [Header("Difficulty Scaling")]
    public float healthIncreasePerWave = 0.15f;
    public float damageIncreasePerWave = 0.10f;
    public float speedIncreasePerWave = 0.05f;

    [Header("Zombie Type Scaling")]
    [Range(0f, 1f)]
    public float walkerChanceWave1 = 0.80f;

    [Range(0f, 1f)]
    public float walkerChanceWave10 = 0.20f;

    private int zombiesAlive;

    void Start()
    {
        upgradeManager = FindFirstObjectByType<UpgradeManager>();

        StartCoroutine(BeginGame());
    }

    IEnumerator BeginGame()
    {
        waveMessage.gameObject.SetActive(true);

        waveMessage.text = "WAVE 1";

        yield return new WaitForSeconds(3f);

        waveMessage.gameObject.SetActive(false);

        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        waveText.text = "Wave " + currentWave;

        int minZombies = 5 + ((currentWave - 1) * 2);
        int maxZombies = 10 + ((currentWave - 1) * 3);

        int zombieCount = Random.Range(minZombies, maxZombies + 1);

        zombiesAlive = zombieCount;

        int activeSpawnPoints = Mathf.Min(2 + currentWave / 2,spawnPoints.Length);

        for (int i = 0; i < zombieCount; i++)
        {
            SpawnZombie();

            yield return new WaitForSeconds(0.4f);
        }
    }

    void SpawnZombie()
    {
        Transform spawnPoint =
            spawnPoints[
                Random.Range(0, spawnPoints.Length)
            ];

        GameObject zombiePrefab;

        float walkerChance = GetWalkerChance();

        if (Random.value < walkerChance)
        {
            zombiePrefab = walkerZombie;
        }
        else
        {
            zombiePrefab = runnerZombie;
        }

        float GetWalkerChance()
        {
            float progress = Mathf.InverseLerp(1f, 10f, currentWave);

            return Mathf.Lerp(
                walkerChanceWave1,
                walkerChanceWave10,
                progress
            );
        }

        GameObject zombie =
            Instantiate(
                zombiePrefab,
                spawnPoint.position,
                spawnPoint.rotation);

        EnemyHealth enemyHealth =
            zombie.GetComponent<EnemyHealth>();

        EnemyAI enemyAI =
            zombie.GetComponent<EnemyAI>();

        //NavMeshAgent agent =
            //zombie.GetComponent<NavMeshAgent>();

        // VIDA
        int newHealth = Mathf.RoundToInt(
            enemyHealth.maxHealth *
            GetDifficultyMultiplier(healthIncreasePerWave)
        );

        enemyHealth.SetHealth(newHealth);

        // DAÑO
        int newDamage = Mathf.RoundToInt(
            enemyAI.damage *
            GetDifficultyMultiplier(damageIncreasePerWave)
        );

        enemyAI.SetDamage(newDamage);

        // VELOCIDAD
        float newSpeed =
            enemyAI.baseSpeed *
            GetDifficultyMultiplier(speedIncreasePerWave);

        enemyAI.SetSpeed(newSpeed);

        // DEBUG
        Debug.Log(
            "Wave " + currentWave +
            " | Walker: " + (walkerChance * 100f).ToString("F0") + "%" +
            " | Runner: " + ((1f - walkerChance) * 100f).ToString("F0") + "%"
        );

        // SPAWNER
        enemyHealth.spawner = this;
    }

    float GetDifficultyMultiplier(float increasePerWave)
    {
        return 1f + ((currentWave - 1) * increasePerWave);
    }

    public void ZombieKilled()
    {
        zombiesAlive--;

        if (zombiesAlive <= 0)
        {
            StartCoroutine(NextWave());
        }
    }

    IEnumerator NextWave()
    {
        waveMessage.gameObject.SetActive(true);

        waveMessage.text = "WAVE CLEARED";

        GameMetricsManager.Instance.EndWave();

        yield return new WaitForSeconds(3f);

        waveMessage.gameObject.SetActive(false);

        upgradeManager.ShowUpgradePanel(this);
    }

    public void StartNextWave()
    {
        currentWave++;

        GameMetricsManager.Instance.RegisterWave(currentWave);

        waveText.text = "Wave " + currentWave;

        waveMessage.gameObject.SetActive(true);

        waveMessage.text = "WAVE " + currentWave;

        StartCoroutine(StartWaveWithDelay());
    }

    IEnumerator StartWaveWithDelay()
    {
        yield return new WaitForSeconds(2f);

        waveMessage.gameObject.SetActive(false);

        StartCoroutine(StartWave());
    }
}
