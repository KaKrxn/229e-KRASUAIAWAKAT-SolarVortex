using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneControllerF22 : MonoBehaviour {
    [Header("Physics")]
    public Rigidbody rb;
    public float thrustPower = 10000f; // แรงขับเครื่องยนต์
    public float maxSpeed = 300f; // ความเร็วสูงสุด

    [Header("Aerodynamics")]
    public float liftPower = 150f;  // แรงยก (Lift)
    public float dragFactor = 0.01f; // แรงต้านอากาศ
    public float rollTorque = 50f;   // แรงบิดสำหรับ Roll
    public float pitchTorque = 40f;  // แรงบิดสำหรับ Pitch
    public float yawTorque = 30f;    // แรงบิดสำหรับ Yaw

    private Vector3 controlInput;
    private float throttleInput;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // ใช้แรงยกแทน
    }

    void Update() {
        HandleInput();
    }

    void FixedUpdate() {
        ApplyThrust();
        ApplyAerodynamics();
        ApplyControls();
    }

    void HandleInput() {
        if (Keyboard.current.wKey.isPressed) throttleInput += 0.01f;
        if (Keyboard.current.sKey.isPressed) throttleInput -= 0.01f;
        throttleInput = Mathf.Clamp(throttleInput, 0f, 1f);

        controlInput = Vector3.zero;
        if (Keyboard.current.aKey.isPressed) controlInput.x = -1f; // Roll Left
        if (Keyboard.current.dKey.isPressed) controlInput.x = 1f;  // Roll Right
        if (Keyboard.current.upArrowKey.isPressed) controlInput.z = 1f; // Pitch Up
        if (Keyboard.current.downArrowKey.isPressed) controlInput.z = -1f; // Pitch Down
        if (Keyboard.current.qKey.isPressed) controlInput.y = -1f; // Yaw Left
        if (Keyboard.current.eKey.isPressed) controlInput.y = 1f;  // Yaw Right
    }

    void ApplyThrust() {
        Vector3 thrustForce = transform.forward * thrustPower * throttleInput;
        rb.AddForce(thrustForce);
    }

    void ApplyAerodynamics() {
        // จำลองแรงยก (Lift)
        float speed = rb.linearVelocity.magnitude;
        Vector3 lift = transform.up * liftPower * speed;
        rb.AddForce(lift);

        // จำลองแรงต้านอากาศ (Drag)
        Vector3 drag = -rb.linearVelocity * dragFactor * speed;
        rb.AddForce(drag);
    }

    void ApplyControls() {
        rb.AddTorque(transform.right * pitchTorque * controlInput.z); // Pitch
        rb.AddTorque(transform.up * yawTorque * controlInput.y); // Yaw
        rb.AddTorque(transform.forward * rollTorque * controlInput.x); // Roll
    }
}
