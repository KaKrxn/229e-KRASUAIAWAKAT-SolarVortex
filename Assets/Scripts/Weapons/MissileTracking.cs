using System.Collections;
using UnityEngine;

public class MissileTracking : MonoBehaviour
{
    [SerializeField] float lifetime = 5f;
    [SerializeField] float speed = 50f;
    [SerializeField] float turningRate = 5f;
    [SerializeField] LayerMask collisionMask;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float coneAngle = 60f;
    [SerializeField] private int minDamage = 40;
    [SerializeField] private int maxDamage = 60;

    private Transform target;
    private Rigidbody rb;
    private bool exploded = false;
    private float timer;
    private PYController pYController;
    public ParticleSystem explosionParticle;
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Awake()
    {
        pYController = GameObject.Find("Player").GetComponent<PYController>();
        explosionParticle.Stop();
        rb = GetComponent<Rigidbody>();
        timer = lifetime;
    }

    private void FixedUpdate()
    {
        if (timer <= 0) Explode();

        timer -= Time.fixedDeltaTime;
        if (timer <= 0) Explode();

        CheckCollision();
        TrackTarget();
        rb.linearVelocity = transform.forward * speed;
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
                int randomDamage = Random.Range(minDamage, maxDamage + 1);

                // ตรวจสอบทุกประเภทของ Enemy
                if (col.TryGetComponent(out EnemyTier1 e1))
                    e1.TakeDamage(randomDamage);
                else if (col.TryGetComponent(out EnemyTier2 e2))
                    e2.TakeDamage(randomDamage);
                else if (col.TryGetComponent(out EnemyTier3 e3))
                    e3.TakeDamage(randomDamage);
                else if (col.TryGetComponent(out BossEnemy boss))
                    boss.TakeDamage(randomDamage);


                Explode(col.transform.position);
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
        rb.MoveRotation(newRotation);
    }

    private void Explode(Vector3? hitPosition = null)
    {
        if (exploded) return;
        exploded = true;

        // ✅ เพิ่ม VFX ตอนโดน
        if (explosionEffect != null)
        {
            Vector3 spawnPos = hitPosition ?? transform.position;
            Instantiate(explosionEffect, spawnPos, Quaternion.identity);
        }
        explosionParticle.Play();
        pYController.PlaySound();
        StartCoroutine(DelayedDestroy(0.1f));
    }


    IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 1, 0.2f);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Vector3 forward = transform.forward * detectionRadius;
        Quaternion leftRay = Quaternion.AngleAxis(-coneAngle / 2, transform.up);
        Quaternion rightRay = Quaternion.AngleAxis(coneAngle / 2, transform.up);

        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, leftRay * forward);
        Gizmos.DrawRay(transform.position, rightRay * forward);
    }
}
