using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHP = 500;  // HP ของบอส
    private int currentHP;

    void Start()
    {
        currentHP = maxHP;  // ตั้งค่า HP ตอนเริ่มเกม
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("Boss HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss Destroyed!");
        Destroy(gameObject); // ทำลายบอส
    }
}