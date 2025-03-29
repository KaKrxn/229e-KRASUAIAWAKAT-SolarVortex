using UnityEngine;
using TMPro;
using System.Drawing;

public class HealthV1 : MonoBehaviour
{
    public int health = 100;
    private float maxTorque = 3;
    float minSpeed = 500;
    float maxSpeed = 800;

    private PYController pyController;
    private int score;
    public int Point = 0;

    // Update is called once per frame
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pyController = GameObject.Find("Player").GetComponent<PYController>();
        rb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
    }
    void Update()
    {
        rb.AddForce(RandomForce(), ForceMode.Impulse);
       
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        int idx = Random.Range(0, Point);
        if (health <= 0)
        {
            Destroy(gameObject);
            pyController.UpdateScore(score);
        }
    }
    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }
    Vector3 RandomForce()
    {
        return Vector3.back * Random.Range(minSpeed, maxSpeed);
    }

    
}
