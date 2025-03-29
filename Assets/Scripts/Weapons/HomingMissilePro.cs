using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HomingMissilePro : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float coneAngle = 60f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private string targetTag = "Enemy";

    [Header("Movement")]
    [SerializeField] private float speed = 50f;
    [SerializeField] private float turnSpeed = 5f;

    [Header("Prediction")]
    [SerializeField] private float predictionMultiplier = 1f;

    [Header("Deviation")]
    [SerializeField] private float deviationAmount = 2f;
    [SerializeField] private float deviationSpeed = 3f;

    [Header("Explosion")]
    [SerializeField] private GameObject explosionEffect;

    private Rigidbody rb;
    private Transform target;
    private Vector3 predictedPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        AcquireTarget();
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            AcquireTarget();
            rb.linearVelocity = transform.forward * speed;
            return;
        }

        PredictTargetPosition();
        AddDeviation();
        RotateTowardsTarget();

        rb.linearVelocity = transform.forward * speed;
    }

    private void AcquireTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, targetMask);
        float closestAngle = coneAngle / 2f;

        foreach (Collider col in hits)
        {
            if (!col.CompareTag(targetTag)) continue;

            Vector3 dirToTarget = (col.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToTarget);

            if (angle <= closestAngle)
            {
                target = col.transform;
                break;
            }
        }
    }

    private void PredictTargetPosition()
    {
        if (target.TryGetComponent<Rigidbody>(out Rigidbody targetRb))
        {
            predictedPosition = target.position + targetRb.linearVelocity * predictionMultiplier;
        }
        else
        {
            predictedPosition = target.position;
        }
    }

    private void AddDeviation()
    {
        Vector3 deviation = new Vector3(
            Mathf.PerlinNoise(Time.time * deviationSpeed, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, Time.time * deviationSpeed) - 0.5f,
            0f) * deviationAmount;

        predictedPosition += transform.TransformDirection(deviation);
    }

    private void RotateTowardsTarget()
    {
        Vector3 dir = (predictedPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime * 100);
        rb.MoveRotation(newRotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (explosionEffect)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 1f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Vector3 forward = transform.forward * detectionRadius;
        Quaternion left = Quaternion.AngleAxis(-coneAngle / 2f, Vector3.up);
        Quaternion right = Quaternion.AngleAxis(coneAngle / 2f, Vector3.up);

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, left * forward);
        Gizmos.DrawRay(transform.position, right * forward);
    }
}
