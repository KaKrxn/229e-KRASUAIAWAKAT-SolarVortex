using UnityEngine;
using System.Collections;

public class BossEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;
    public float moveSpeed = 3f;
    public float waitAtPointTime = 1.5f;

    [Header("Boss Stats")]
    public int maxHP = 300;
    private int currentHP;
    public int contactDamage = 20;

    [Header("Phase 1: Bullet Burst Settings")]
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private float burstCooldown = 3f;
    [SerializeField] private int bulletsPerBurst = 10;
    [SerializeField] private float bulletSpeed = 30f;
    [SerializeField] private float bulletLifetime = 3f;
    [SerializeField] private int bulletDamage = 10;

    [Header("Phase 2: Laser Attack Settings")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform[] laserFirePoints;
    [SerializeField] private float laserChargeTime = 2f;
    [SerializeField] private float laserDuration = 1.5f;
    [SerializeField] private float laserCooldown = 5f;
    [SerializeField] private int laserDamage = 50;

    [Header("Phase 3: Advanced Movement & Dash")]
    [SerializeField] private float increasedMoveSpeed = 6f;
    [SerializeField] private float dashCooldown = 7f;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.4f;
    [SerializeField] private float missileDodgeChance = 0.3f;
    [SerializeField] private float dodgeDetectionRadius = 10f;
    [SerializeField] private LayerMask missileLayer;
    public int Point;
    public ParticleSystem explosionParticle;
    public AudioClip crashSfx;
    private AudioSource playerAudio;
    private Transform player;
    private Vector3 targetPosition;
    private PYController pyController;
    private bool isMoving = false;
    private bool isDashing = false;

    private void Start()
    {
        currentHP = maxHP;
        explosionParticle.Stop();
        pyController = GameObject.Find("Player").GetComponent<PYController>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        playerAudio = GetComponent<AudioSource>();
        StartCoroutine(MoveLoop());
        StartCoroutine(Phase1_BulletBurst());
        StartCoroutine(Phase2_LaserAttack());
        StartCoroutine(Phase3_DashAndDodge());
        StartCoroutine(MissileDodgeSystem());
    }

    private IEnumerator MoveLoop()
    {
        while (true)
        {
            if (!isDashing)
            {
                float targetX = Random.Range(minX, maxX);
                float targetY = Random.Range(minY, maxY);
                targetPosition = new Vector3(targetX, targetY, transform.position.z);

                isMoving = true;

                while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                isMoving = false;
                yield return new WaitForSeconds(waitAtPointTime);
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator Phase1_BulletBurst()
    {
        while (true)
        {
            for (int i = 0; i < bulletsPerBurst; i++)
            {
                foreach (Transform point in firePoints)
                {
                    if (player != null)
                    {
                        Vector3 dir = (player.position - point.position).normalized;
                        GameObject bullet = Instantiate(bulletPrefab, point.position, Quaternion.LookRotation(dir));
                        Rigidbody rb = bullet.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.linearVelocity = dir * bulletSpeed;
                        }

                        BulletBehavior bulletScript = bullet.GetComponent<BulletBehavior>();
                        if (bulletScript != null)
                        {
                            bulletScript.SetDamage(bulletDamage);
                        }

                        Destroy(bullet, bulletLifetime);
                    }
                }
                yield return new WaitForSeconds(fireRate);
            }
            yield return new WaitForSeconds(burstCooldown);
        }
    }

    private IEnumerator Phase2_LaserAttack()
    {
        while (true)
        {
            if (player != null)
            {
                foreach (Transform laserPoint in laserFirePoints)
                {
                    Vector3 lockOnDirection = (player.position - laserPoint.position).normalized;
                    Quaternion lockRotation = Quaternion.LookRotation(lockOnDirection);
                    laserPoint.rotation = lockRotation;
                }

                yield return new WaitForSeconds(laserChargeTime);

                foreach (Transform laserPoint in laserFirePoints)
                {
                    GameObject laser = Instantiate(laserPrefab, laserPoint.position, laserPoint.rotation);
                    LaserBoss laserScript = laser.GetComponent<LaserBoss>();
                    if (laserScript != null)
                    {
                        laserScript.SetDamage(laserDamage);
                        laserScript.SetDuration(laserDuration);
                    }

                    Destroy(laser, laserDuration);
                }
            }

            yield return new WaitForSeconds(laserCooldown);
        }
    }

    private IEnumerator Phase3_DashAndDodge()
    {
        while (true)
        {
            yield return new WaitForSeconds(dashCooldown);

            if (player != null)
            {
                Vector3 dashTarget = new Vector3(player.position.x, player.position.y, transform.position.z);
                Vector3 returnPosition = new Vector3(
                    Random.Range(minX, maxX),
                    Random.Range(minY, maxY),
                    transform.position.z
                );

                isDashing = true;
                float elapsed = 0f;
                Vector3 startPos = transform.position;

                while (elapsed < dashDuration)
                {
                    transform.position = Vector3.Lerp(startPos, dashTarget, elapsed / dashDuration);
                    elapsed += Time.deltaTime;
                    yield return null;
                }

                transform.position = dashTarget;
                yield return new WaitForSeconds(0.5f);

                elapsed = 0f;
                while (elapsed < dashDuration)
                {
                    transform.position = Vector3.Lerp(dashTarget, returnPosition, elapsed / dashDuration);
                    elapsed += Time.deltaTime;
                    yield return null;
                }

                transform.position = returnPosition;
                isDashing = false;
            }
        }
    }

    private IEnumerator MissileDodgeSystem()
    {
        while (true)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, dodgeDetectionRadius, missileLayer);

            if (hits.Length > 0 && !isDashing)
            {
                if (Random.value < missileDodgeChance)
                {
                    Debug.Log("🚀 Boss detected missile and is dodging!");
                    StartCoroutine(DodgeAway());
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator DodgeAway()
    {
        isDashing = true;

        Vector3 dodgeTarget = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            transform.position.z
        );

        float elapsed = 0f;
        Vector3 startPos = transform.position;

        while (elapsed < dashDuration)
        {
            transform.position = Vector3.Lerp(startPos, dodgeTarget, elapsed / dashDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = dodgeTarget;
        isDashing = false;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {   
        explosionParticle.Play();
        if (pyController != null)
        {
            int idx = Random.Range(10, Point);
            WaveManager waveManager = FindObjectOfType<WaveManager>();
            int difficulty = waveManager != null ? waveManager.difficultyLevel : 1;
            int scoreToAdd = idx * difficulty;
            pyController.UpdateScore(scoreToAdd);
        
        }
        StartCoroutine(DelayedDestroy(2f));
    }
    IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PYController player = other.GetComponent<PYController>();
            if (player != null)
            {
                player.TakeDamage(contactDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, transform.position.z), new Vector3(Mathf.Abs(maxX - minX), Mathf.Abs(maxY - minY), 0.1f));
    }
}

// 

