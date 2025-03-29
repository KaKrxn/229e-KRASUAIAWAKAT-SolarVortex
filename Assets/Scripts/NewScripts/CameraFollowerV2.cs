using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// CameraFollowerV2 class is responsible for following the player with a certain offset.
/// It supports Camera view switching:
/// - "C" for Rear View (behind the player)
/// - "V" for Front View (in front of the player)
/// </summary>
public class CameraFollowerV2 : MonoBehaviour
{
    public Transform target; // ตัวละครที่กล้องจะติดตาม
    public Vector3 rearOffset = new Vector3(0, 5, -10); 
    public Vector3 frontOffset = new Vector3(0, 3, 5);

    public bool CameraOpen;
    
    
    public float smoothSpeed = 5f; 

    private Vector3 currentOffset; 
    private Vector3 velocity = Vector3.zero; 
    private Quaternion desiredRotation; // การหมุนของกล้องที่ต้องการ

    void Start()
    {
        CameraOpen = false;
        currentOffset = rearOffset; // เริ่มต้นที่มุมมองด้านหลัง
        desiredRotation = Quaternion.identity;
        gameObject.SetActive(CameraOpen);
    }

    void LateUpdate()
    {
        if (target == null) return;
        //gameObject.SetActive(CameraOpen);
        
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            currentOffset = rearOffset;
            desiredRotation = target.rotation * Quaternion.Euler(0, 180, 0);
            
        }

        
        // if (Keyboard.current.vKey.wasPressedThisFrame)
        // {
        //     currentOffset = frontOffset;
        //     desiredRotation = target.rotation;
            
            
        // }

        // คำนวณตำแหน่งที่ต้องการให้กล้องไปอยู่
        Vector3 desiredPosition = target.position + target.rotation * currentOffset;

        // ใช้ SmoothDamp เพื่อให้กล้องเคลื่อนที่นุ่มนวล
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed * Time.deltaTime);

        // กล้องหันหน้าตาม Player เสมอ
        transform.LookAt(target);
        //transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothSpeed * Time.deltaTime);
    }
}
