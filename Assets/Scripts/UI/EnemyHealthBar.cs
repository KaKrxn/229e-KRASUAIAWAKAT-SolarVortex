using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Transform target;               // ศัตรูที่ติดตาม
    public Vector3 offset = new Vector3(0, 2f, 0); // ความสูงเหนือหัว
    public Slider slider;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // แปลงตำแหน่งโลก → พิกัดหน้าจอ
        Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);

        // ซ่อนถ้าอยู่หลังกล้อง
        if (screenPos.z < 0)
        {
            slider.gameObject.SetActive(false);
        }
        else
        {
            slider.gameObject.SetActive(true);
            transform.position = screenPos;
        }
    }

    public void SetHealth(float current, float max)
    {
        slider.value = current / max;
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }
}
