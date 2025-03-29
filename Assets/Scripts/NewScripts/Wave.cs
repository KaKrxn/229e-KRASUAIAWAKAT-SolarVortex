using UnityEngine;

[System.Serializable]
public class Wave
{
    [Header("Enemy Settings")]
    public int Enemies = 5;              // จำนวนศัตรูใน wave นี้
    public float delayStart = 2f;        // รอก่อนเริ่ม wave
    public float spawnInterval = 1f;     // เวลาห่างกันในการ spawn ศัตรูแต่ละตัว

    [Header("Asteroid Settings")]
    public int Asteroid = 0;
    public float AsteroidInterval = 1f;
    public float minX = -9f, maxX = 9f;
    public float minZ = -10f, maxZ = 10f;

    [Header("Boss Settings")]
    public bool spawnBoss = false;       // กำหนดว่า wave นี้จะมี boss ไหม
    public Transform bossSpawnPoint;     // จุดที่ boss จะ spawn (ลากใน Inspector)
}
