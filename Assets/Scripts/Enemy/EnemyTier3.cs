using UnityEngine;
using System.Collections;

public class EnemyTier3 : MonoBehaviour
{
    [Header("Movement")]
    public float minX = -10f, maxX = 10f;
    public float minY = -5f, maxY = 5f;
    public float moveSpeed = 3f;
    public float waitTime = 1.5f;

    [Header("Laser Attack")]
    public GameObject laserPrefab;
    public Transform[] laserFirePoints;
    public float laserChargeTime = 1.5f;
    public float laserDuration = 2f;
    public float laserCooldown = 4f;
    public int laserDamage = 25;

    [Header("Stats")]
    public int maxHP = 100;
    private int currentHP;
    public int contactDamage = 10;
    public ParticleSystem explosionParticle;
    public AudioClip crashSfx;
    private Transform player;
    private AudioSource playerAudio;
    public int Point;
    private PYController pyController;

    private void Start()
    {
        explosionParticle.Stop();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        pyController = GameObject.Find("Player").GetComponent<PYController>();
        playerAudio = GetComponent<AudioSource>();
        currentHP = maxHP;
        StartCoroutine(MoveRandomly());
        StartCoroutine(FireLaserRoutine());
    }

    private IEnumerator MoveRandomly()
    {
        while (true)
        {
            Vector3 target = new Vector3(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY),
                transform.position.z);

            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator FireLaserRoutine()
    {
        while (true)
        {
            if (player != null)
            {
                foreach (Transform point in laserFirePoints)
                {
                    Vector3 dir = (player.position - point.position).normalized;
                    point.rotation = Quaternion.LookRotation(dir);
                }
            }

            yield return new WaitForSeconds(laserChargeTime);

            foreach (Transform point in laserFirePoints)
            {
                GameObject laser = Instantiate(laserPrefab, point.position, point.rotation);
                LaserBehavior laserScript = laser.GetComponent<LaserBehavior>();
                if (laserScript != null)
                {
                    laserScript.SetDamage(laserDamage);
                    laserScript.SetDuration(laserDuration);
                }

                Destroy(laser, laserDuration);
            }

            yield return new WaitForSeconds(laserCooldown);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            playerAudio.PlayOneShot(crashSfx);
            explosionParticle.Play();
            Die();
        }
    }

    private void Die()
    {
        int idx = Random.Range(10, Point);
        WaveManager waveManager = FindObjectOfType<WaveManager>();
        int difficulty = waveManager != null ? waveManager.difficultyLevel : 1;
        int scoreToAdd = idx * difficulty;
        
        if (pyController != null)
        {
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
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(
            new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, transform.position.z),
            new Vector3(Mathf.Abs(maxX - minX), Mathf.Abs(maxY - minY), 0.2f)
        );
    }
}
