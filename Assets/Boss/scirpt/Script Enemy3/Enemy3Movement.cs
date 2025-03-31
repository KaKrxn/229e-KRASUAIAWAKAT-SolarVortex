using UnityEngine;

public class Enemy3Movement : MonoBehaviour
{
    public float speed = 2f; // ความเร็วของบอส
    public float changeDirectionTime = 2f; // เวลาที่จะเปลี่ยนทิศทาง

    public float leftLimit = -4f;
    public float rightLimit = 4f;
    public float lowerLimit = -2f;
    public float upperLimit = 2f;

    private Vector3 moveDirection;
    private float timer;

    void Start()
    {
        ChangeDirection(); // กำหนดทิศทางเริ่มต้นแบบสุ่ม
    }

    void Update()
    {
        // เคลื่อนที่ไปตามทิศทางที่สุ่มไว้
        transform.position += moveDirection * speed * Time.deltaTime;

        // ตรวจสอบว่าถึงขอบเขตหรือยัง
        if (transform.position.x >= rightLimit || transform.position.x <= leftLimit)
            moveDirection.x *= -1; // กลับทิศทางแนว X

        if (transform.position.y >= upperLimit || transform.position.y <= lowerLimit)
            moveDirection.y *= -1; // กลับทิศทางแนว Y

        // เปลี่ยนทิศทางทุกๆ changeDirectionTime วินาที
        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            ChangeDirection();
            timer = 0;
        }
    }

    void ChangeDirection()
    {
        // สุ่มค่า -1 หรือ 1 เพื่อเปลี่ยนทิศทางในแนว X และ Y
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);

        // ป้องกันไม่ให้หยุดนิ่ง (ต้องมีการเคลื่อนที่แน่ๆ)
        if (Mathf.Abs(randomX) < 0.3f) randomX = Mathf.Sign(Random.Range(-1f, 1f));
        if (Mathf.Abs(randomY) < 0.3f) randomY = Mathf.Sign(Random.Range(-1f, 1f));

        moveDirection = new Vector3(randomX, randomY, 0).normalized; // กำหนดทิศทางใหม่แบบสุ่ม
    }
}