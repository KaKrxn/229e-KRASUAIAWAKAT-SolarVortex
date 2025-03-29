using UnityEngine;

public class EnemyTier1 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveForce = 5f;             // แรงเคลื่อนที่
    public float moveDistance = 5f;          // ระยะทางที่เคลื่อนที่ไปกลับ

    [Header("Attack Settings")]
    public float attackCooldown = 2f;        // คูลดาวน์ระหว่างการยิง
    public float bulletForce = 5000f;         // แรงกระสุน
    public int damage = 10;                  // ดาเมจที่ทำได้

    [Header("Health Settings")]
    public int maxHP = 100;                  // HP สูงสุด
    private int currentHP;

    [Header("References")]
    public GameObject bulletPrefab;          // กระสุน
    public Transform shootPoint;             // จุดยิงกระสุน
    public Transform player;                 // เป้าหมายผู้เล่น

    private Rigidbody rb;
    private Vector3 startPos;
    private bool movingRight = true;
    private float attackTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        currentHP = maxHP;
    }

    void FixedUpdate() // ใช้ FixedUpdate เพราะเกี่ยวกับฟิสิกส์
    {
        Move();
    }

    void Update()
    {
        Attack();
    }

    // ======= เคลื่อนที่ด้วย AddForce =========
    void Move()
    {
        float direction = movingRight ? 1f : -1f;
        rb.AddForce(Vector3.right * direction * moveForce);

        // เช็คขอบเขตการเคลื่อนที่
        if (movingRight && transform.position.x >= startPos.x + moveDistance)
        {
            movingRight = false;
        }
        else if (!movingRight && transform.position.x <= startPos.x - moveDistance)
        {
            movingRight = true;
        }
    }

    // ======= ยิงกระสุนด้วย AddForce ==========
    void Attack()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f && player != null)
        {
            // คำนวณทิศทาง
            Vector3 direction = (player.position - shootPoint.position).normalized;

            // สร้างกระสุน
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                bulletRb.AddForce(direction * bulletForce);
            }

            // กำหนด Damage
            EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = damage;
            }

            // รีเซ็ตคูลดาวน์
            attackTimer = attackCooldown;
        }
    }

    // ======= รับดาเมจ =======
    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
