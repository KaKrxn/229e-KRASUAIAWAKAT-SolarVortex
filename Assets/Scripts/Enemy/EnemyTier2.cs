using UnityEngine;
using System.Collections;

public class EnemyTier2 : MonoBehaviour
{
    public float moveSpeed = 2f;  // ความเร็วในการเคลื่อนที่
    public Vector2 minPosition;   // ค่าต่ำสุดของ X, Y
    public Vector2 maxPosition;   // ค่าสูงสุดของ X, Y

    public GameObject bulletPrefab; // กระสุนที่ใช้ยิง
    public Transform firePoint;    // จุดที่กระสุนถูกยิงออกไป
    public float bulletForce = 5000f;

    public float fireDelay = 2f;   // ดีเลย์ในการยิง
    public int damage = 10;        // ค่าความเสียหายที่สร้าง
    public int maxHP = 50;         // ค่า HP สูงสุด
    private int currentHP; 
    
    [SerializeField] private GameObject healthBarPrefab;
    private EnemyHealthBar healthBarUI;        // ค่า HP ปัจจุบัน

    public Vector3 targetPosition;
    public Transform player;

    public int Point;

    private PYController pyController;
    
    void Start()
    {
        currentHP = maxHP;
        GameObject canvas = GameObject.Find("CanvasHUD"); // หรือชื่อ Canvas ของคุณ
        if (canvas && healthBarPrefab)
        {
            GameObject bar = Instantiate(healthBarPrefab, canvas.transform);
            healthBarUI = bar.GetComponent<EnemyHealthBar>();
            healthBarUI.SetTarget(transform);
            healthBarUI.SetHealth(currentHP, maxHP);
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        pyController = GameObject.Find("Player").GetComponent<PYController>();
        StartCoroutine(MoveRandomly());
        StartCoroutine(ShootAtPlayer());
    }


    void Update()
    {
        // RaycastHit hit;

        // Vector3 shootDirection = transform.forward; 
        // Debug.DrawRay(firePoint.position, shootDirection * 50f, Color.red);
    }
    IEnumerator MoveRandomly()
    {
        while (true)
        {
            // สุ่มค่าพิกัดเป้าหมาย
            targetPosition = new Vector2(
                Random.Range(minPosition.x, maxPosition.x),
                Random.Range(minPosition.y, maxPosition.y)
            );

            // เคลื่อนที่ไปยังเป้าหมาย
            while ((Vector3)transform.position != targetPosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // หยุดชั่วคราวก่อนสุ่มเป้าหมายใหม่
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    IEnumerator ShootAtPlayer()
    {
        while (true)
        {
            if (player != null)
            {
                // หมุนไปทางผู้เล่น
                Vector3 direction = (player.position - firePoint.position).normalized;
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
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

               
               
                // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                // firePoint.rotation = Quaternion.Euler(0, 0, angle);

                // // ยิงกระสุน
                // GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                // bullet.GetComponent<EnemyBulletTier2>().SetDamage(damage);

                // รอเวลาตามค่า delay ก่อนยิงครั้งต่อไป
                yield return new WaitForSeconds(fireDelay);
            }
            else
            {
                yield return null;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (healthBarUI != null)
            healthBarUI.SetHealth(currentHP, maxHP);
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        int idx = Random.Range(10, Point);
        pyController.UpdateScore(idx);
        Destroy(gameObject); // ทำลาย Enemy เมื่อ HP หมด
    }
}