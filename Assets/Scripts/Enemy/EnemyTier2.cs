using UnityEngine;
using System.Collections;

public class EnemyTier2 : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Vector2 minPosition;
    public Vector2 maxPosition;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 5000f;

    public float fireDelay = 2f;
    public int damage = 10;
    public int maxHP = 50;
    private int currentHP;

    public Vector3 targetPosition;
    public Transform player;

    public int Point;
    public ParticleSystem explosionParticle;
    public AudioClip crashSfx;
    private PYController pyController;
    private AudioSource playerAudio;

    void Start()
    {
        currentHP = maxHP;
        explosionParticle.Stop();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        pyController = GameObject.Find("Player").GetComponent<PYController>();
        playerAudio = GetComponent<AudioSource>();
        StartCoroutine(MoveRandomly());
        StartCoroutine(ShootAtPlayer());
    }

    IEnumerator MoveRandomly()
    {
        while (true)
        {
            targetPosition = new Vector2(
                Random.Range(minPosition.x, maxPosition.x),
                Random.Range(minPosition.y, maxPosition.y)
            );

            while ((Vector3)transform.position != targetPosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    IEnumerator ShootAtPlayer()
    {
        while (true)
        {
            if (player != null)
            {
                Vector3 direction = (player.position - firePoint.position).normalized;
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

                if (bulletRb != null)
                {
                    bulletRb.AddForce(direction * bulletForce);
                }

                EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
                if (bulletScript != null)
                {
                    bulletScript.damage = damage;
                }

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
        Debug.Log(currentHP);
        if (currentHP <= 0)
        {       
            playerAudio.PlayOneShot(crashSfx);
            explosionParticle.Play();
            
            Die();
        }
    }

    void Die()
    {   
        int idx = Random.Range(10, Point);
        WaveManager waveManager = FindObjectOfType<WaveManager>();
        int difficulty = waveManager != null ? waveManager.difficultyLevel : 1;
        int scoreToAdd = idx * difficulty;
        pyController.UpdateScore(scoreToAdd);
        
        Destroy(gameObject);
    }
}
