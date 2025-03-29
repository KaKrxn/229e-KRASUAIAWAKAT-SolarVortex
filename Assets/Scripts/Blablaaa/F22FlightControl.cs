using System;
using UnityEngine;
using UnityEngine.Serialization;
public class F22FlightControl : MonoBehaviour
{
     public float maxThrust = 1000f;  // แรงขับสูงสุด
    public float thrustChangeRate = 50f; // อัตราการเพิ่ม/ลดแรงขับ
    public float liftForce = 500f;  // แรงยก
    public float pitchSpeed = 50f;  // ความเร็วในการ Pitch (เงย/ก้ม)
    public float yawSpeed = 20f;    // ความเร็วในการ Yaw (หมุนซ้าย/ขวา)
    public float rollSpeed = 40f;   // ความเร็วในการ Roll (เอียงปีก)
    [SerializeField] private float enginePower = -20f;
    
    private float currentThrust = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // ควบคุม Throttle (W/S)
        if (Input.GetKey(KeyCode.W))
            currentThrust = Mathf.Clamp(currentThrust + thrustChangeRate * Time.deltaTime, 0, maxThrust);
        if (Input.GetKey(KeyCode.S))
            currentThrust = Mathf.Clamp(currentThrust - thrustChangeRate * Time.deltaTime, 0, maxThrust);

        // ควบคุม Pitch (ลูกศรขึ้น/ลง)
        float pitch = 0;
        if (Input.GetKey(KeyCode.UpArrow)) pitch = -pitchSpeed;
        if (Input.GetKey(KeyCode.DownArrow)) pitch = pitchSpeed;

        // ควบคุม Yaw (A/D)
        float yaw = 0;
        if (Input.GetKey(KeyCode.A)) yaw = -yawSpeed;
        if (Input.GetKey(KeyCode.D)) yaw = yawSpeed;

        // ควบคุม Roll (Q/E)
        float roll = 0;
        if (Input.GetKey(KeyCode.Q)) roll = rollSpeed;
        if (Input.GetKey(KeyCode.E)) roll = -rollSpeed;

        // ใช้แรงหมุนกับเครื่องบิน
        rb.AddTorque(transform.right * pitch * Time.deltaTime);
        rb.AddTorque(transform.up * yaw * Time.deltaTime);
        rb.AddTorque(transform.forward * roll * Time.deltaTime);
    }

    void FixedUpdate()
    {
        // เพิ่มแรงขับเคลื่อนไปข้างหน้า
        rb.AddForce(transform.forward * currentThrust * Time.deltaTime, ForceMode.Acceleration);

        // เพิ่มแรงยกเมื่อเคลื่อนที่เร็วพอ
        if (rb.linearVelocity.magnitude > 50f) // Takeoff speed
        {
            rb.AddForce(transform.up * liftForce * Time.deltaTime, ForceMode.Acceleration);


        }

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(transform.right * enginePower);
            
           // Vector3 Lift = Vector3.Project(rb.linearVelocity, transform.right);
           // rb.AddForce(transform.up * Lift.magnitude * liftBooter);
            
        }
    }
}
