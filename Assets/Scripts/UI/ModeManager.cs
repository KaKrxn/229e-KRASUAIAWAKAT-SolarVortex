using UnityEngine;
using UnityEngine.UI;

public class ModeManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject titleScreen;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    [Header("Difficulty Settings")]
    public int difficultyLevel = 1; // 1 = Easy, 2 = Medium, 3 = Hard

    private WaveManager waveManager;

    private void Awake()
    {
        easyButton.onClick.AddListener(() => StartGame(1));
        mediumButton.onClick.AddListener(() => StartGame(2));
        hardButton.onClick.AddListener(() => StartGame(3));
    }

    private void Start()
    {
        GameObject waveObj = GameObject.Find("WaveSpawnManager");
        if (waveObj != null)
        {
            waveManager = waveObj.GetComponent<WaveManager>();
        }
        else
        {
            Debug.LogError("❌ WaveManager not found in scene!");
        }

        titleScreen.SetActive(true); // เปิดหน้าจอ Title ตอนเริ่ม
    }

    void StartGame(int difficulty)
    {
        difficultyLevel = difficulty;

        // ส่งค่าความยากไปให้ WaveManager
        if (waveManager != null)
        {
            waveManager.difficultyLevel = difficulty;
            StartCoroutine(waveManager.WaveSpawner());
        }

        titleScreen.SetActive(false);
        Debug.Log("🎮 Start Game at difficulty: " + difficulty);
    }
}