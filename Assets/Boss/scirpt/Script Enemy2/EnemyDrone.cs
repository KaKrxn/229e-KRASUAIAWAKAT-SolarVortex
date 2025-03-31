using UnityEngine;

public class EnemyDrone : MonoBehaviour
{
    public Transform player;
    public float speed = 10f;
    public float maxSpeed = 20f;
    public int health = 20;
    public Rigidbody rb;
    public GameObject explosionEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.AddForce(direction * speed, ForceMode.VelocityChange);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }

            Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ชนกับ: " + other.gameObject.name); // ✅ ตรวจสอบว่าโดนชนจริงไหม

        if (other.CompareTag("Player"))
        {
            Debug.Log("ชน Player แล้วระเบิด!"); // ✅ Debug เช็กว่าถึงตรงนี้ไหม

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(40);
            }

            Explode();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
