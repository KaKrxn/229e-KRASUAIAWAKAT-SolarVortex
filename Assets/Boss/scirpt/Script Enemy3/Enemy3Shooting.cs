using UnityEngine;

public class Enemy3Shooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint1, firePoint2;
    public Transform player;  // �����蹷�����������
    public float fireRate = 2f;
    private float nextFireTime = 0f;

    void Update()
    {
        if (player != null && Time.time >= nextFireTime)
        {
            LockOnTarget();
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void LockOnTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // ��ع�ش�ԧ����ѹ价ҧ������
        firePoint1.rotation = lookRotation;
        firePoint2.rotation = lookRotation;
    }

    void Fire()
    {
        Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);
        Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);
    }
}