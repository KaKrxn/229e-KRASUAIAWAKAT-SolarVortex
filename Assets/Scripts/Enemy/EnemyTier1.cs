using System.Collections;
using UnityEngine;

public class EnemyTier1 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float trackSpeed = 3f;
    public float dashSpeed = 10f;
    public float dashCooldown = 2f;

    [Header("Combat Settings")]
    public int maxHP = 50;
    private int currentHP;

    public int Point = 30; // คะแนนพื้นฐาน
    private bool isDashing = false;
    private bool isInCooldown = false;
    private bool hasHitPlayer = false;

    [Header("References")]
    [SerializeField] private GameObject explosionEffect;

    private Vector3 startPoint;
    private float originalZ;

    private Transform player;
    private PYController pyController;

    void Start()
    {
        currentHP = maxHP;

        player = GameObject.FindWithTag("Player")?.transform;
        pyController = GameObject.FindWithTag("Player")?.GetComponent<PYController>();

        if (pyController == null)
            Debug.LogWarning("⚠ PYController not found! Please check the Player object.");

        startPoint = transform.position;
        originalZ = transform.position.z;

        StartCoroutine(TrackAndDashLoop());
    }

    IEnumerator TrackAndDashLoop()
    {
        while (true)
        {
            while (!isDashing && !isInCooldown && player != null &&
                   Vector2.Distance(transform.position, new Vector2(player.position.x, player.position.y)) > 0.1f)
            {
                Vector3 targetPos = new Vector3(player.position.x, player.position.y, originalZ);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, trackSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            if (!isInCooldown && player != null)
            {
                isDashing = true;
                hasHitPlayer = false;
                yield return StartCoroutine(DashToPlayer());
            }

            yield return null;
        }
    }

    IEnumerator DashToPlayer()
    {
        Vector3 dashTarget = new Vector3(transform.position.x, transform.position.y, player.position.z);

        while (!hasHitPlayer && Vector3.Distance(transform.position, dashTarget) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
            yield return null;
        }

        if (!hasHitPlayer)
        {
            isInCooldown = true;
            transform.position = new Vector3(startPoint.x, startPoint.y, originalZ);
            yield return new WaitForSeconds(dashCooldown);
            isInCooldown = false;
        }

        isDashing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDashing && other.CompareTag("Player"))
        {
            hasHitPlayer = true;
            Debug.Log("💥 Hit Player!");
            Destroy(gameObject); // ใส่ระบบให้ Player เสีย HP ได้ตรงนี้
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log($"EnemyTier1 took {amount} damage. Current HP = {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        int idx = Random.Range(10, Point);
        WaveManager waveManager = FindObjectOfType<WaveManager>();
        int difficulty = waveManager != null ? waveManager.difficultyLevel : 1;
        int scoreToAdd = idx * difficulty;

        if (pyController != null)
        {
            pyController.UpdateScore(scoreToAdd);
        }

        Destroy(gameObject);
    }
}
