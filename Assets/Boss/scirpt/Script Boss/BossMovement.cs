using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float speed = 2f; 
    public float changeDirectionTime = 2f; 

    public float leftLimit = -4f;
    public float rightLimit = 4f;
    public float lowerLimit = -2f;
    public float upperLimit = 2f;

    private Vector3 moveDirection;
    private float timer;

    void Start()
    {
        ChangeDirection(); 
    }

    void Update()
    {
       
        transform.position += moveDirection * speed * Time.deltaTime;

        
        if (transform.position.x >= rightLimit || transform.position.x <= leftLimit)
            moveDirection.x *= -1; 

        if (transform.position.y >= upperLimit || transform.position.y <= lowerLimit)
            moveDirection.y *= -1; 

        
        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            ChangeDirection();
            timer = 0;
        }
    }

    void ChangeDirection()
    {
        
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);

        
        if (Mathf.Abs(randomX) < 0.3f) randomX = Mathf.Sign(Random.Range(-1f, 1f));
        if (Mathf.Abs(randomY) < 0.3f) randomY = Mathf.Sign(Random.Range(-1f, 1f));

        moveDirection = new Vector3(randomX, randomY, 0).normalized;
    }
}