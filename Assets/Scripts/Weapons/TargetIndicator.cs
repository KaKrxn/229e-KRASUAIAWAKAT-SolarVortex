using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour
{
    public GameObject indicatorPrefab; // ตั้งค่า Prefab ของ Indicator ใน Inspector
    private GameObject indicatorInstance;
    private Transform target;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        GameObject canvas = GameObject.Find("CanvasHUD");
        if (canvas && indicatorPrefab != null)
        {
            indicatorInstance = Instantiate(indicatorPrefab, canvas.transform);
            indicatorInstance.SetActive(false);
        }
        else
        {
            Debug.LogError("❌ CanvasHUD or indicatorPrefab not found!");
        }
    }

    private void Update()
    {
        if (target == null || indicatorInstance == null) return;

        // แปลงตำแหน่งศัตรูเป็นพิกัดบนจอ
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(target.position);
        
        // ถ้าอยู่ข้างหลังกล้องให้ซ่อน
        if (screenPosition.z < 0)
        {
            indicatorInstance.SetActive(false);
        }
        else
        {
            indicatorInstance.SetActive(true);
            indicatorInstance.transform.position = screenPosition;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        // 💥 ป้องกัน crash
        if (indicatorInstance == null)
        {
            Debug.LogWarning("⚠️ indicatorInstance is null! Ensure it was instantiated in Start().");
            return;
        }

        indicatorInstance.SetActive(target != null);
    }

    private void OnDestroy()
    {
        if (indicatorInstance) Destroy(indicatorInstance);
    }
}
