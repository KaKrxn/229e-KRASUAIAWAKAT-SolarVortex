using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 10;     // ดาเมจ
    public float lifeTime = 30f; // อายุของกระสุนก่อนหายไปเอง

    void Start()
    {
        Destroy(gameObject, lifeTime); // ทำลายตัวเองเมื่อครบเวลา
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PYController playerHealth = other.GetComponent<PYController>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // สร้างดาเมจให้ Player
            }
            Destroy(gameObject); // ทำลายกระสุนเมื่อโดนเป้าหมาย
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject); // กระสุนโดนกำแพง หรือวัตถุอื่น ๆ
        }
    }
}
