using UnityEngine;

public class LaserBoss : MonoBehaviour
{
    
    private int damage;
    private float duration;
    [SerializeField] private float laserSpeed = 100f; // เพิ่มความเร็ว laser
    private Rigidbody rb;

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
    

    public void SetDuration(float time)
    {
        duration = time;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    

    private void Start()
    {
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * laserSpeed;
        }
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.red, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PYController player = other.GetComponent<PYController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }

}
