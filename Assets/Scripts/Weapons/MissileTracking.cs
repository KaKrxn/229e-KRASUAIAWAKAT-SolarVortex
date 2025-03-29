// using System.Collections;
// using UnityEngine;

// public class MissileTracking : MonoBehaviour
// {
//     [SerializeField] float lifetime = 5f;
//     [SerializeField] float speed = 50f;
//     [SerializeField] float trackingAngle = 60f;
//     [SerializeField] float turningRate = 5f;
//     [SerializeField] LayerMask collisionMask;
//     [SerializeField] GameObject explosionEffect;
//     [SerializeField] private float detectionRadius = 5f;
//     [SerializeField] private float coneAngle = 60f;

//     [Header("Lock Target UI")]
//     [SerializeField] private RectTransform lockAreaRect;
//     [SerializeField] private Camera mainCamera;
//     [SerializeField] private GameObject lockIndicator; // optional indicator for the locked target

//     private Transform owner;
//     private Transform target;
//     private Rigidbody rb;
//     private bool exploded = false;
//     private float timer;

//     public void SetTarget(Transform newTarget)
//     {
//         if (newTarget == null || !IsTargetInLockArea(newTarget))
//         {
//             Debug.Log("❌ Target not in lock area");
//             return;
//         }

//         target = newTarget;

//         TargetIndicator indicator = target.GetComponent<TargetIndicator>();
//         if (indicator != null)
//         {
//             indicator.SetTarget(target);
//         }

//         // Show lock indicator if available
//         if (lockIndicator != null)
//         {
//             lockIndicator.SetActive(true);
//             lockIndicator.transform.position = mainCamera.WorldToScreenPoint(target.position);
//         }

//         Debug.Log("🔒 Locked on target: " + target.name);
//     }

//     private void Awake()
//     {
//         rb = GetComponent<Rigidbody>();
//         timer = lifetime;
//         if (mainCamera == null) mainCamera = Camera.main;
//     }

//     private void Update()
//     {
//         if (target != null)
//         {
//             Debug.DrawLine(transform.position, target.position, Color.green);

//             if (lockIndicator != null)
//             {
//                 lockIndicator.transform.position = mainCamera.WorldToScreenPoint(target.position);
//             }
//         }
//     }

//     private void FixedUpdate()
//     {
//         if (exploded) return;

//         timer -= Time.fixedDeltaTime;
//         if (timer <= 0) Explode();

//         CheckCollision();
//         TrackTarget();
//         rb.linearVelocity = transform.forward * speed;
//     }

//     private void CheckCollision()
//     {
//         Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, collisionMask);

//         foreach (var col in hits)
//         {
//             Vector3 dirToTarget = (col.transform.position - transform.position).normalized;
//             float angle = Vector3.Angle(transform.forward, dirToTarget);

//             if (angle <= coneAngle / 2f)
//             {
//                 if (col.CompareTag("Enemy"))
//                 {
//                     Debug.Log("🎯 Hit target in cone: " + col.name);
//                     Explode();
//                     break;
//                 }
//             }
//         }
//     }

//     private void TrackTarget()
//     {
//         if (target == null) return;

//         Vector3 direction = (target.position - transform.position).normalized;
//         Quaternion targetRotation = Quaternion.LookRotation(direction);
//         Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningRate * Time.fixedDeltaTime * 100);
//         rb.MoveRotation(newRotation);
//     }

//     private void Explode()
//     {
//         if (exploded) return;
//         exploded = true;

//         if (explosionEffect != null)
//         {
//             Instantiate(explosionEffect, transform.position, Quaternion.identity);
//         }

//         if (lockIndicator != null)
//         {
//             lockIndicator.SetActive(false);
//         }

//         Destroy(gameObject);
//     }

//     private bool IsTargetInLockArea(Transform enemy)
//     {
//         if (mainCamera == null || lockAreaRect == null) return false;

//         Vector3 screenPos = mainCamera.WorldToScreenPoint(enemy.position);
//         if (screenPos.z < 0) return false;

//         return RectTransformUtility.RectangleContainsScreenPoint(lockAreaRect, screenPos);
//     }

//     private void OnDrawGizmosSelected()
//     {
//         Gizmos.color = new Color(1, 0, 1, 0.2f);
//         Gizmos.DrawWireSphere(transform.position, detectionRadius);

//         Vector3 forward = transform.forward * detectionRadius;
//         Quaternion leftRay = Quaternion.AngleAxis(-coneAngle / 2, transform.up);
//         Quaternion rightRay = Quaternion.AngleAxis(coneAngle / 2, transform.up);

//         Gizmos.color = Color.magenta;
//         Gizmos.DrawRay(transform.position, leftRay * forward);
//         Gizmos.DrawRay(transform.position, rightRay * forward);
//     }
// }

using System.Collections;

using UnityEngine;

public class MissileTracking : MonoBehaviour

{
    [SerializeField] float lifetime = 5f;
    [SerializeField] float speed = 50f;
    [SerializeField] private int damage = 10;
    [SerializeField] float trackingAngle = 60f;
    [SerializeField] float turningRate = 5f;
    [SerializeField] LayerMask collisionMask;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] private float detectionRadius = 5f;     // รัศมีการตรวจจับ
    [SerializeField] private float coneAngle = 60f;          // มุมของกรวย (องศา)

    private Transform owner;
    private Transform target;
    private Rigidbody rb;
    private bool exploded = false;
    private float timer;

    public void SetTarget(Transform newTarget)
{
    target = newTarget;

    // หา TargetIndicator บนศัตรู
    TargetIndicator indicator = target.GetComponent<TargetIndicator>();

    if (indicator != null)
    {
        indicator.SetTarget(target); // อัพเดต Indicator ให้ชี้ไปที่เป้าหมายใหม่
    }
}


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        timer = lifetime;
    }
    private void Update()
{
    if (target != null)
    {
        Debug.DrawLine(transform.position, target.position, Color.green); // เส้นเขียวไปเป้า
    }
}

    private void FixedUpdate()
    {
        if (exploded) return;

        timer -= Time.fixedDeltaTime;
        if (timer <= 0) Explode();

        CheckCollision();
        TrackTarget();
        rb.linearVelocity = transform.forward * speed;
        if (target != null)
        {
            Debug.DrawLine(transform.position, target.position, Color.green); // เส้นเขียวไปเป้า
        }
    }

    private void CheckCollision()
    {
        // RaycastHit hit;
        // if (Physics.Raycast(transform.position, transform.forward, out hit, 100f, collisionMask))
        // {
        //     if (hit.collider.CompareTag("Enemy"))
        //     {
        //         Explode();
        //     }
        // }

        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, collisionMask);

        foreach (var col in hits)
        {
            Vector3 dirToTarget = (col.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToTarget);

            if (angle <= coneAngle / 2f)
            {
                if (col.CompareTag("Enemy"))
                {
                    Debug.Log("🎯 Hit target in cone: " + col.name);
                    Explode();
                    break;
                }
            }
        }
    }

    private void TrackTarget()
    {
        if (target == null) return;

        // Vector3 directionToTarget = (target.position - transform.position).normalized;
        // float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        // if (angleToTarget > trackingAngle)
        // {
        //     target = null; // ���������ش�ҡ���еԴ���
        //     return;
        // }

        // Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningRate * Time.fixedDeltaTime * 100);

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningRate * Time.fixedDeltaTime * 100);
        rb.MoveRotation(newRotation); // ใช้ Rigidbody หมุน
    }

    private void Explode()
    {
        if (exploded) return;
        exploded = true;

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 🎯 ตรวจจับศัตรูในระยะระเบิด
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, collisionMask);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
            // 👇 ลองหาว่ามี EnemyTier2 ไหม
            EnemyTier2 enemy = hit.GetComponent<EnemyTier2>();
                if (enemy != null)
                {   
                    enemy.TakeDamage(damage);
                    Debug.Log($"💥 {hit.name} took {damage} damage from missile");
                }
            }
    }

    Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 1, 0.2f); // ม่วงแบบโปร่ง
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Vector3 forward = transform.forward * detectionRadius;
        Quaternion leftRay = Quaternion.AngleAxis(-coneAngle / 2, transform.up);
        Quaternion rightRay = Quaternion.AngleAxis(coneAngle / 2, transform.up);

        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, leftRay * forward);
        Gizmos.DrawRay(transform.position, rightRay * forward);
    }   


    
}


// // public class MissileTracking : MonoBehaviour
// // {
// //     [SerializeField] float speed = 50f;
// //     [SerializeField] float trackingAngle = 60f;
// //     [SerializeField] float turningRate = 5f;
// //     [SerializeField] float lifetime = 5f;
// //     [SerializeField] LayerMask collisionMask;
    
// //     private Transform target;
// //     private Rigidbody rb;
// //     private float timer;

// //     public void SetTarget(Transform newTarget)
// //     {
// //         target = newTarget;
// //     }

// //     private void Awake()
// //     {
// //         rb = GetComponent<Rigidbody>();
// //         timer = lifetime;
// //     }

// //     private void FixedUpdate()
// //     {
// //         if (target == null) return;

// //         timer -= Time.fixedDeltaTime;
// //         if (timer <= 0) Destroy(gameObject);

// //         TrackTarget();
// //         rb.linearVelocity = transform.forward * speed;
// //     }

// //     private void TrackTarget()
// //     {
// //         if (target == null) return;

// //         Vector3 directionToTarget = (target.position - transform.position).normalized;
// //         float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

// //         if (angleToTarget > trackingAngle)
// //         {
// //             target = null;
// //             return;
// //         }

// //         Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
// //         transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningRate * Time.fixedDeltaTime * 100);
// //     }
// // }


// // public class MissileTracking : MonoBehaviour
// // {
// //     [SerializeField] float speed = 50f;
// //     [SerializeField] float turningRate = 5f;
// //     private Transform target;
// //     private Rigidbody rb;
// //     private bool isTracking = false;

// //     public void SetTarget(Transform newTarget)
// //     {
// //         target = newTarget;
// //         isTracking = true;
// //     }

// //     private void Awake()
// //     {
// //         rb = GetComponent<Rigidbody>();
// //     }

// //     private void FixedUpdate()
// //     {
// //         if (isTracking && target != null)
// //         {
// //             Vector3 directionToTarget = (target.position - transform.position).normalized;
// //             Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
// //             transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningRate * Time.fixedDeltaTime * 100);
// //         }

// //         rb.linearVelocity = transform.forward * speed;
// //     }
// // }