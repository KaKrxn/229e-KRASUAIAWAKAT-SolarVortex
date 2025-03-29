using UnityEngine;

public class Targets : MonoBehaviour
{
    [SerializeField] private string targetName;
    private Rigidbody rb;

    public string Name => targetName;
    public Vector3 Position => rb.position;
    public Vector3 Velocity => rb.linearVelocity; // เปลี่ยนจาก linearVelocity เป็น velocity ปกติ

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
