using UnityEngine;

public class AddTqure : MonoBehaviour
{
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("❌ Rigidbody not found on " + gameObject.name);
            return;
        }

        rb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
    }

    float RandomTorque()
    {
        return Random.Range(-3f, 3f); // หรือใช้ maxTorque ถ้ามี
    }
}
