using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public RectTransform pitchIndicator; // UI แสดง Pitch
    public RectTransform rollIndicator;  // UI แสดง Roll
    public Transform aircraft;           // อ้างอิงเครื่องบิน

    void Update()
    {
        if (aircraft == null) return;

        // ดึงค่ามุม Pitch และ Roll ของเครื่องบิน
        float pitch = aircraft.eulerAngles.z;
        float roll = aircraft.eulerAngles.x;

        // ปรับให้อยู่ในช่วง -180 ถึง 180
        if (pitch > 180) pitch -= 360;
        if (roll > 180) roll -= 360;

        // อัปเดต UI
        pitchIndicator.anchoredPosition = new Vector2(0, pitch * 2); // ขึ้น-ลง
        rollIndicator.localRotation = Quaternion.Euler(0, 0, -roll); // หมุนซ้าย-ขวา
    }
}
  