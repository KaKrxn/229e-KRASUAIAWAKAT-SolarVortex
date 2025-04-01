using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private int damage;

    public void SetDamage(int dmg)
    {
        damage = dmg;
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
            Destroy(gameObject);
        }
    }
}
