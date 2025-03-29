using UnityEngine;

public class EnemyBulletTier2 : MonoBehaviour
{
    public float speed = 5f;  // ความเร็วกระสุน

    public Vector3 Move;
    private int damage;

    public void SetDamage(int amount)
    {
        damage = amount;
    }

    void Update()
    {
        //transform.Translate(Move * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PYController>()?.TakeDamage(damage);
            Destroy(gameObject); // ทำลายกระสุนหลังจากชน
        }
    }
}
