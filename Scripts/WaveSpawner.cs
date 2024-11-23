using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemyPrefabs;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    public Transform spawnPoint;
    public Transform[] waypoints;
    public float timeBetweenWaves = 5f;
    public Text waveCountdownText;
    public Text waveNumberText;

    private int currentWaveIndex = 0;
    private float countdown = 2f;
    private bool isSpawning = false;

    private void Update()
    {
        if (currentWaveIndex >= waves.Length)
            return;

        if (!isSpawning)
        {
            countdown -= Time.deltaTime;
            countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
            if (waveCountdownText != null)
                waveCountdownText.text = $"Next Wave: {countdown:00.00}";

            if (countdown <= 0f)
            {
                StartCoroutine(SpawnWave());
                isSpawning = true;
            }
        }
    }

    IEnumerator SpawnWave()
    {
        Wave wave = waves[currentWaveIndex];
        if (waveNumberText != null)
            waveNumberText.text = $"Wave: {currentWaveIndex + 1}";

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        currentWaveIndex++;
        countdown = timeBetweenWaves;
        isSpawning = false;
    }

    void SpawnEnemy(Wave wave)
    {
        int randomEnemyIndex = Random.Range(0, wave.enemyPrefabs.Length);
        GameObject enemyGO = Instantiate(wave.enemyPrefabs[randomEnemyIndex], spawnPoint.position, spawnPoint.rotation);
        Enemy enemy = enemyGO.GetComponent<Enemy>();
        enemy.Initialize(waypoints);
    }
}