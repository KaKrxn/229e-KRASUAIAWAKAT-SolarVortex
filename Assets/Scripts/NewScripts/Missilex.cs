using UnityEngine;

public class Missilex : MonoBehaviour
{
    public float speed = 20f; // ความเร็วของมิสไซล์
    public float trackingSpeed = 5f; // ความเร็วในการหมุนติดตามเป้า
    public float lifeTime = 5f; // เวลาในการทำลายตัวเอง

    private Transform target; // เป้าหมายที่ต้องการติดตาม
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime); // ทำลายตัวเองหลังจากเวลาผ่านไป
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            // คำนวณทิศทางไปยังเป้าหมาย
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // ค่อย ๆ หมุนไปหาเป้า
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, trackingSpeed * Time.deltaTime);
        }

        // เคลื่อนที่ไปข้างหน้าเสมอ
        rb.linearVelocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // ถ้ามิสไซล์ชนศัตรู
        {
            Destroy(gameObject); // ทำลายมิสไซล์
            Destroy(other.gameObject); // ทำลายศัตรู
        }
    }
}
