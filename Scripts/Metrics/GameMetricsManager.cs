using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMetricsManager : MonoBehaviour
{
    public static GameMetricsManager Instance;

    public MetricsData metrics = new MetricsData();

    private float gameStartTime;

    private float waveStartTime;

    private bool waitingForRestart = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        gameStartTime = Time.time;

        waveStartTime = Time.time;
    }

    public void RegisterRestart()
    {
        metrics.restartCount++;

        Debug.Log("Reinicios: " + metrics.restartCount);

        waitingForRestart = true;
    }

    public void RegisterShot()
    {
        metrics.shotsFired++;

        Debug.Log("Disparos: " + metrics.shotsFired);
    }

    public void RegisterHit()
    {
        metrics.shotsHit++;

        Debug.Log("Impactos: " + metrics.shotsHit);
    }

    public void RegisterKill()
    {
        metrics.zombiesKilled++;

        Debug.Log("Zombies eliminados: " + metrics.zombiesKilled);
    }

    public void RegisterDamage(int damage)
    {
        metrics.damageTaken += damage;

        Debug.Log("Daño recibido: " + metrics.damageTaken);
    }

    public void RegisterReload()
    {
        metrics.reloads++;

        Debug.Log("Recargas: " + metrics.reloads);
    }

    public void RegisterUpgrade(string upgrade)
    {
        metrics.upgradesChosen.Add(upgrade);

        Debug.Log("Mejora: " + upgrade);
    }

    public void RegisterWave(int wave)
    {
        metrics.maxWaveReached = wave;

        Debug.Log("Oleada: " + wave);
    }

    public void EndWave()
    {
        float waveTime = Time.time - waveStartTime;

        metrics.waveTimes.Add(waveTime);

        Debug.Log("Tiempo oleada: " + waveTime);

        waveStartTime = Time.time;
    }

    public void FinishGame()
    {
        metrics.totalPlayTime =
            Time.time - gameStartTime;

        CalculateAccuracy();

        Debug.Log("Tiempo total: " + metrics.totalPlayTime);

        Debug.Log("Precisión: " + metrics.accuracy);

        CSVExporter.Export(metrics);
    }

    void CalculateAccuracy()
    {
        if (metrics.shotsFired == 0)
        {
            metrics.accuracy = 0;
            return;
        }

        metrics.accuracy =
            (float)metrics.shotsHit /
            metrics.shotsFired * 100f;
    }

    public void ResetMetrics()
    {
        int restartCount = metrics.restartCount;

        metrics = new MetricsData();

        metrics.restartCount = restartCount;

        gameStartTime = Time.time;

        waveStartTime = Time.time;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (waitingForRestart)
        {
            ResetMetrics();

            waitingForRestart = false;
        }
    }
}
