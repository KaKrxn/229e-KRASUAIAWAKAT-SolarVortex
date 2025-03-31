using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public GameObject bulletPrefab;  // Prefab กระสุน
    public Transform firePoint_Left1, firePoint_Left2, firePoint_Right1, firePoint_Right2;
    public float fireRate = 2f; // ความถี่ในการยิง (ยิงทุกๆ 2 วินาที)
    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Fire()
    {
        Instantiate(bulletPrefab, firePoint_Left1.position, firePoint_Left1.rotation);
        Instantiate(bulletPrefab, firePoint_Left2.position, firePoint_Left2.rotation);
        Instantiate(bulletPrefab, firePoint_Right1.position, firePoint_Right1.rotation);
        Instantiate(bulletPrefab, firePoint_Right2.position, firePoint_Right2.rotation);
    }
}
