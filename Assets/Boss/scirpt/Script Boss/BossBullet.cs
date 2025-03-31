using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 500f;  // �����ç�ͧ����ع
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.right * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject); // ����ع��������ͪ�������
        }
    }
}
