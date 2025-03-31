using UnityEngine;

public class Enemy3Movement : MonoBehaviour
{
    public float speed = 2f; // �������Ǣͧ���
    public float changeDirectionTime = 2f; // ���ҷ�������¹��ȷҧ

    public float leftLimit = -4f;
    public float rightLimit = 4f;
    public float lowerLimit = -2f;
    public float upperLimit = 2f;

    private Vector3 moveDirection;
    private float timer;

    void Start()
    {
        ChangeDirection(); // ��˹���ȷҧ�������Ẻ����
    }

    void Update()
    {
        // ����͹���仵����ȷҧ����������
        transform.position += moveDirection * speed * Time.deltaTime;

        // ��Ǩ�ͺ��Ҷ֧�ͺࢵ�����ѧ
        if (transform.position.x >= rightLimit || transform.position.x <= leftLimit)
            moveDirection.x *= -1; // ��Ѻ��ȷҧ�� X

        if (transform.position.y >= upperLimit || transform.position.y <= lowerLimit)
            moveDirection.y *= -1; // ��Ѻ��ȷҧ�� Y

        // ����¹��ȷҧ�ء� changeDirectionTime �Թҷ�
        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            ChangeDirection();
            timer = 0;
        }
    }

    void ChangeDirection()
    {
        // ������� -1 ���� 1 ��������¹��ȷҧ��� X ��� Y
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);

        // ��ͧ�ѹ��������ش��� (��ͧ�ա������͹������)
        if (Mathf.Abs(randomX) < 0.3f) randomX = Mathf.Sign(Random.Range(-1f, 1f));
        if (Mathf.Abs(randomY) < 0.3f) randomY = Mathf.Sign(Random.Range(-1f, 1f));

        moveDirection = new Vector3(randomX, randomY, 0).normalized; // ��˹���ȷҧ����Ẻ����
    }
}