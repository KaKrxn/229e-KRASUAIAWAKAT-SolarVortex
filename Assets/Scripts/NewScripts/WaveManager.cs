using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public TextMeshProUGUI loopText;

    [Header("Prefabs")]
    public GameObject[] enemyPrefabs; // Enemy Tier 1, Tier 2
    public GameObject[] asteroidPrefabs;
    public GameObject bossPrefab;
    public Transform[] spawnPoints;

    [Header("Wave Settings")]
    public List<Wave> waves;
    private List<Wave> randomizedWaves;

    [Header("Difficulty")]
    public int difficultyLevel = 1; // Easy: 1, Medium: 2, Hard: 3
    public float loopDifficultyMultiplier = 0.2f;
    private int loopCount = 1;

    private int currentWaveIndex = 0;
    private int activeEnemies = 0;

    private PYController player;

    void Start()
    {
        player = FindObjectOfType<PYController>();
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
        List<Wave> copy = new List<Wave>(waves);
        randomizedWaves = copy.OrderBy(x => Random.value).ToList();
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

            if (wave.spawnBoss && bossPrefab != null)
            {
                Instantiate(bossPrefab, wave.bossSpawnPoint.position, Quaternion.identity);
                yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
            }

            currentWaveIndex++;
            UpdateLoopUI();
        }
    }

    IEnumerator SpawnEnemies(Wave wave)
    {
        for (int i = 0; i < wave.Enemies; i++)
        {
            int prefabIndex = Random.Range(0, enemyPrefabs.Length);
            int spawnIndex = Random.Range(0, spawnPoints.Length);

            GameObject enemyObj = Instantiate(enemyPrefabs[prefabIndex], spawnPoints[spawnIndex].position, Quaternion.identity);

            // Apply scaling based on loop
            float scaleFactor = 1 + loopDifficultyMultiplier * loopCount;

            // Tier 1
            var enemy1 = enemyObj.GetComponent<EnemyTier1>();
            if (enemy1 != null)
            {
                enemy1.maxHP = Mathf.RoundToInt(enemy1.maxHP * scaleFactor);
                //enemy1.moveSpeed *= scaleFactor;
            }

            // Tier 2
            var enemy2 = enemyObj.GetComponent<EnemyTier2>();
            if (enemy2 != null)
            {
                enemy2.maxHP = Mathf.RoundToInt(enemy2.maxHP * scaleFactor);
                enemy2.damage = Mathf.RoundToInt(enemy2.damage * scaleFactor);
                enemy2.moveSpeed *= scaleFactor;
                enemy2.fireDelay = Mathf.Max(0.2f, enemy2.fireDelay / scaleFactor);
                enemy2.bulletForce *= scaleFactor;
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
        // Difficulty is applied dynamically during enemy spawn
    }

    void UpdateLoopUI()
    {
        loopText.text = "" + (loopCount + 1);
    }
} 
