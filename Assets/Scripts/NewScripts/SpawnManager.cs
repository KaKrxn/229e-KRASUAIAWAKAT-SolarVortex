using UnityEngine;

public class SpawnManager : MonoBehaviour
{
     // [1] declare a public GameObject array for animal prefabs
    public GameObject[] Obstacle;
    private int ObstacleIndex;
    public float spawnRangeX = 15;
    public float spawnRangeY = 15;
    public float TimeCooldown;
    void Start()
    {
        InvokeRepeating(nameof(Spawn), 1, TimeCooldown);
    }

    void Spawn()
    {
       ObstacleIndex = Random.Range(0, Obstacle.Length);
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), Random.Range(-spawnRangeY, spawnRangeY), transform.position.z);
        Instantiate(Obstacle[ObstacleIndex],spawnPos,Obstacle[ObstacleIndex].transform.rotation);
    }
}
