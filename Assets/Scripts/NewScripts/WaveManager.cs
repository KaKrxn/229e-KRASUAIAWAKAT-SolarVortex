using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public TextMeshProUGUI loopText;
    [Header("Prefabs")]
    public GameObject[] enemyPrefabs;
    public GameObject[] asteroidPrefabs;
    public GameObject bossPrefab;
    public Transform[] spawnPoints;

    [Header("Wave Settings")]
    public List<Wave> waves;
    private List<Wave> randomizedWaves;

    [Header("Difficulty")] 
    public int difficultyLevel = 1; // Easy: 1, Medium: 2, Hard: 3
    public float loopDifficultyMultiplier = 0.2f;
    private int loopCount = 0;

    private int currentWaveIndex = 0;
    private int activeEnemies = 0;

    private PYController player;

    void Start()
    {
        player = FindObjectOfType<PYController>();
        // ResetAndRandomizeWaves();
        // StartCoroutine(WaveSpawner());
    }

    public void StartWaves()
    {
        ResetAndRandomizeWaves();
        StartCoroutine(WaveSpawner());
    }

    void EnemyDestroyed(GameObject enemy)
    {
        activeEnemies--;
    }

    void ResetAndRandomizeWaves()
    {
        randomizedWaves = waves.OrderBy(x => Random.value).ToList();
        currentWaveIndex = 0;
    }

    public IEnumerator WaveSpawner()
    {
        while (true)
        {
            if (currentWaveIndex >= randomizedWaves.Count)
            {
                loopCount++;
                ApplyDifficultyScaling();
                ResetAndRandomizeWaves();

                // รี HP Player
                if (player != null)
                {
                    player.RestoreFullHP();
                }
            }

            Wave wave = randomizedWaves[currentWaveIndex];
            yield return new WaitForSeconds(wave.delayStart);

            yield return StartCoroutine(SpawnEnemies(wave));
            StartCoroutine(SpawnAsteroids(wave));

            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);

            // Spawn Boss ถ้ามี
            if (wave.spawnBoss && bossPrefab != null)
            {
                Instantiate(bossPrefab, wave.bossSpawnPoint.position, Quaternion.identity);
                yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
            }

            currentWaveIndex++;
        }
    }

    IEnumerator SpawnEnemies(Wave wave)
    {
        for (int i = 0; i < wave.Enemies; i++)
        {
            int prefabIndex = Random.Range(0, enemyPrefabs.Length);
            int spawnIndex = Random.Range(0, spawnPoints.Length);

            GameObject enemyObj = Instantiate(enemyPrefabs[prefabIndex], spawnPoints[spawnIndex].position, Quaternion.identity);

            var enemy = enemyObj.GetComponent<EnemyTier2>();
            if (enemy != null)
            {
                enemy.maxHP = Mathf.RoundToInt(enemy.maxHP * (1 + loopDifficultyMultiplier * loopCount));
                enemy.damage = Mathf.RoundToInt(enemy.damage * (1 + loopDifficultyMultiplier * loopCount));
                enemy.moveSpeed *= (1 + loopDifficultyMultiplier * loopCount);
                enemy.fireDelay = Mathf.Max(0.2f, enemy.fireDelay / (1 + loopDifficultyMultiplier * loopCount));
                enemy.bulletForce *= (1 + loopDifficultyMultiplier * loopCount);
            }

            var destroyScript = enemyObj.GetComponent<DestroyOutOfBounds>();
            if (destroyScript != null)
            {
                destroyScript.OnDestroyEvent += EnemyDestroyed;
            }

            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    IEnumerator SpawnAsteroids(Wave wave)
    {
        for (int i = 0; i < wave.Asteroid; i++)
        {
            int index = Random.Range(0, asteroidPrefabs.Length);
            Vector3 spawnPos = new Vector3(
                Random.Range(wave.minX, wave.maxX),
                Random.Range(wave.minZ, wave.maxZ),
                transform.position.z
            );

            Instantiate(asteroidPrefabs[index], spawnPos, asteroidPrefabs[index].transform.rotation);
            yield return new WaitForSeconds(wave.AsteroidInterval);
        }
    }

    void ApplyDifficultyScaling()
    {
        Debug.Log("⚙️ Wave loop complete! Increasing difficulty...");
        // Logic สำหรับปรับความยากเพิ่มขึ้นต่อรอบ (ทำใน SpawnEnemies)
    }
    void UpdateLoopUI() {
    loopText.text = "Loop: " + (loopCount + 1);
    }
}
