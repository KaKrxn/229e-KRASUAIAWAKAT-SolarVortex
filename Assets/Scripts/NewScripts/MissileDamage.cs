using UnityEngine;

public class MissileDamage : MonoBehaviour
{
   public int attackPoint = 30;

    void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponent<HealthV1>();
        if (health != null)
        {
            health.TakeDamage(attackPoint);
        }
        Destroy(gameObject);

        // if (other.TryGetComponent<HealthV1>(out var health))
        // {
        //     health.TakeDamage(attackPoint);
        // }
        // Destroy(gameObject);
    }
}
