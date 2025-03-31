using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHP = 500;  // HP �ͧ���
    private int currentHP;

    void Start()
    {
        currentHP = maxHP;  // ��駤�� HP �͹�������
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
        Destroy(gameObject); // ����º��
    }
}