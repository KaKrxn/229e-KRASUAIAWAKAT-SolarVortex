using System.Collections;
using UnityEngine;

public class MissileTracking : MonoBehaviour
{
    [SerializeField] float lifetime = 5f;
    [SerializeField] float speed = 50f;
    [SerializeField] float trackingAngle = 60f;
    [SerializeField] float turningRate = 5f;
    [SerializeField] LayerMask collisionMask;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] private float detectionRadius = 5f;     // รัศมีการตรวจจับ
    [SerializeField] private float coneAngle = 60f;          // มุมของกรวย (องศา)
    [SerializeField] private int minDamage = 40;             // ดาเมจต่ำสุดของมิสไซล์
    [SerializeField] private int maxDamage = 60;             // ดาเมจสูงสุดของมิสไซล์

    private Transform owner;
    private Transform target;
    private Rigidbody rb;
    private bool exploded = false;
    private float timer;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        timer = lifetime;
    }

    private void Update()
    {
        if (target != null)
        {
            Debug.DrawLine(transform.position, target.position, Color.green); // เส้นเขียวไปเป้า
        }
    }

    private void FixedUpdate()
    {
        if (exploded) return;

        timer -= Time.fixedDeltaTime;
        if (timer <= 0) Explode();

        CheckCollision();
        TrackTarget();
        rb.linearVelocity = transform.forward * speed;

        if (target != null)
        {
            Debug.DrawLine(transform.position, target.position, Color.green); // เส้นเขียวไปเป้า
        }
    }

    private void CheckCollision()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, collisionMask);

        foreach (var col in hits)
        {
            Vector3 dirToTarget = (col.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToTarget);

            if (angle <= coneAngle / 2f && col.CompareTag("Enemy"))
            {
                Debug.Log("🎯 Hit target in cone: " + col.name);

                int randomDamage = Random.Range(minDamage, maxDamage + 1);

                // ✅ ลองหา EnemyTier2
                EnemyTier2 enemy = col.GetComponent<EnemyTier2>();
                if (enemy != null)
                {
                    enemy.TakeDamage(randomDamage);
                }

                Explode();
                break;
            }
        }
    }

    private void TrackTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningRate * Time.fixedDeltaTime * 100);
        rb.MoveRotation(newRotation); // ใช้ Rigidbody หมุน
    }

    private void Explode()
    {
        if (exploded) return;
        exploded = true;

        //Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 1, 0.2f); // ม่วงแบบโปร่ง
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Vector3 forward = transform.forward * detectionRadius;
        Quaternion leftRay = Quaternion.AngleAxis(-coneAngle / 2, transform.up);
        Quaternion rightRay = Quaternion.AngleAxis(coneAngle / 2, transform.up);

        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, leftRay * forward);
        Gizmos.DrawRay(transform.position, rightRay * forward);
    }
}
