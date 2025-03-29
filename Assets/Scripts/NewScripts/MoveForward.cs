using UnityEngine;
using UnityEngine.EventSystems;

public class MoveForward : MonoBehaviour
{
    public float speed = 40.0f;
    private float maxTorque = 3;
    float minSpeed = 500;
    float maxSpeed = 800;

    // Update is called once per frame
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.AddForce(RandomForce(),ForceMode.Impulse);
        rb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
    }
    void Update()
    {
        rb.AddForce(RandomForce(), ForceMode.Impulse);
        //rb.AddForce(Vector3.forward * maxSpeed);
        //transform.Translate(speed * Time.deltaTime * Vector3.forward);
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
