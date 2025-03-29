using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class WaveManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] enemyPrefab;
    public GameObject[] Obstacle;
    public Transform[] spawnPoints;

    private int ObstacleIndex;
    public float spawnRangeX = 15;
    public float spawnRangeY = 15;

    
    private int enemyCount;
    private int astoridCount;

    [Header("Wave Settings")]
    public List<Wave> waves;

    [Header("Difficulty")]
    public int difficultyLevel = 1;
    
    private int currentWaveIndex = 0;
    private List<int> selectedSpawnPoints = new List<int>();
    private int activeEnemies = 0;

    void Start()
    {
        //StartCoroutine(WaveSpawner());
        
    }

    void EnemyDestroyed(GameObject enemy)
    {
        activeEnemies--;
    }

    public IEnumerator WaveSpawner()
    {
        
        while (currentWaveIndex < waves.Count)
        {
            Wave wave = waves[currentWaveIndex];
            
            //selectedSpawnPoints = GetRandomSpawnPoints(wave.Asteriod);
            
            yield return new WaitForSeconds(wave.delayStart);

            StartCoroutine(SpawnEnemies(wave));
            StartCoroutine(SpawnAstorid (wave));

            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);

            currentWaveIndex++;
        }
    }

    IEnumerator SpawnEnemies(Wave wave)
    {
        // activeEnemies = wave.Enemies; // กำหนดจำนวน enemy ใน wave

        // for (int i = 0; i < wave.Enemies; i++)
        // {
        //     // int spawnIndex = selectedSpawnPoints[Random.Range(0, selectedSpawnPoints.Count)];
        //     GameObject enemy = Instantiate(enemyPrefab[enemyCount], spawnPoints[0].position, Quaternion.identity);

        //     var destroyScript = enemy.GetComponent<DestroyOutOfBounds>();
        //     if (destroyScript != null)
        //     {
        //         destroyScript.OnDestroyEvent += EnemyDestroyed; // ติดตามการถูกทำลาย
        //     }

        //     yield return new WaitForSeconds(wave.spawnInterval);
        // }
            activeEnemies = wave.Enemies;

        for (int i = 0; i < wave.Enemies; i++)
        {
            GameObject enemyObj = Instantiate(enemyPrefab[enemyCount], spawnPoints[0].position, Quaternion.identity);

            // 🔥 ปรับค่าศัตรูตามระดับความยาก (ยกเว้น Boss)
            if (!enemyObj.CompareTag("Boss"))
            {
                var enemy = enemyObj.GetComponent<EnemyTier2>();
                if (enemy != null)
                {
                    enemy.maxHP = Mathf.RoundToInt(enemy.maxHP * difficultyLevel);
                    enemy.damage = Mathf.RoundToInt(enemy.damage * difficultyLevel);
                    enemy.moveSpeed *= difficultyLevel;
                    enemy.fireDelay = Mathf.Max(0.2f, enemy.fireDelay / difficultyLevel); // ยิงเร็วขึ้นในระดับที่สูงขึ้น
                    enemy.bulletForce *= difficultyLevel;
                }
            }

            var destroyScript = enemyObj.GetComponent<DestroyOutOfBounds>();
            if (destroyScript != null)
            {
                destroyScript.OnDestroyEvent += EnemyDestroyed;
            }

            yield return new WaitForSeconds(wave.spawnInterval);
        }
    
    }
    
    void Spawn()
    {
        ObstacleIndex = Random.Range(0, Obstacle.Length);
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), Random.Range(-spawnRangeY, spawnRangeY), transform.position.z);
        Instantiate(Obstacle[ObstacleIndex],spawnPos,Obstacle[ObstacleIndex].transform.rotation);
    }
    IEnumerator SpawnAstorid(Wave wave)
    {
        for (int i = 0; i < wave.Asteriod; i++)
        {
            
            Debug.Log("SpawnAstoriod");  
            Spawn();
            yield return new WaitForSeconds(wave.AstoriodTnterval);
        }
    }

    // List<int> GetRandomSpawnPoints(int count)
    // {
    //     List<int> availablePoints = new List<int>();
    //     for (int i = 0; i < spawnPoints.Length; i++) availablePoints.Add(i);

    //     List<int> selectedPoints = new List<int>();
    //     for (int i = 0; i < count; i++)
    //     {
    //         int randomIndex = Random.Range(0, availablePoints.Count);
    //         selectedPoints.Add(availablePoints[randomIndex]);
    //         availablePoints.RemoveAt(randomIndex);
    //     }
    //     return selectedPoints;
    // }
}

