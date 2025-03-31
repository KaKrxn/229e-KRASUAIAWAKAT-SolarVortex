using UnityEngine;
using System.Collections;

public class EnemyTier1 : MonoBehaviour
{
    public float trackSpeed = 3f;
    public float dashSpeed = 10f;
    public float dashCooldown = 2f;

    public int maxHP = 50;
    private int currentHP;

    public int Point;

    private Vector3 startPoint;
    private float originalZ;
    private bool isDashing = false;
    private bool isInCooldown = false;
    private bool hasHitPlayer = false;

    private Transform player;
    private PYController pyController;

    void Start()
    {
        currentHP = maxHP;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        pyController = GameObject.Find("Player").GetComponent<PYController>();

        startPoint = transform.position;
        originalZ = transform.position.z;

        StartCoroutine(TrackAndDashLoop());
    }

    IEnumerator TrackAndDashLoop()
    {
        while (true)
        {
            // ขยับ X/Y ไปหาผู้เล่นจนกว่าจะตรงกัน (หรือเกือบ)
            while (!isDashing && !isInCooldown && Vector2.Distance(transform.position, new Vector2(player.position.x, player.position.y)) > 0.1f)
            {
                Vector3 targetPos = new Vector3(player.position.x, player.position.y, originalZ);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, trackSpeed * Time.deltaTime);
                yield return null;
            }

            // รอสักแป๊บก่อนพุ่ง
            yield return new WaitForSeconds(0.5f);

            if (!isInCooldown)
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

        // ถ้าไม่โดน
        if (!hasHitPlayer)
        {
            Debug.Log("❌ Missed. Going back...");
            isInCooldown = true;

            // กลับจุดเดิม
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
            Debug.Log("💥 Hit Player!");
            hasHitPlayer = true;
            Destroy(gameObject);
            // TODO: ลด HP ผู้เล่นถ้าต้องการ
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        int idx = Random.Range(10, Point);
        WaveManager waveManager = FindObjectOfType<WaveManager>();
        int difficulty = waveManager != null ? waveManager.difficultyLevel : 1;
        int scoreToAdd = idx * difficulty;
        if (pyController != null)
        {
            pyController.UpdateScore(scoreToAdd);
        };
        Destroy(gameObject);
    }
}
