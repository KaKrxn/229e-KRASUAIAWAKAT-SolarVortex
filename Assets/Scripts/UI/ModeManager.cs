using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModeManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject titleScreen;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    public Button MenuButton;
    public Button ResetButton;

    [SerializeField] GameObject BGMode;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject HUDGame;

    [Header("Difficulty Settings")]
    public int difficultyLevel = 1; // 1 = Easy, 2 = Medium, 3 = Hard

    private WaveManager waveManager;

    private void Awake()
    {
        easyButton.onClick.AddListener(() => StartGame(1));
        mediumButton.onClick.AddListener(() => StartGame(2));
        hardButton.onClick.AddListener(() => StartGame(3));
        MenuButton.onClick.AddListener(() => BackToMainMenu());
        ResetButton.onClick.AddListener(() => ResetGame());
    }

    private void Start()
    {
        titleScreen.SetActive(true);
        HUDGame.SetActive(false);
        Player.SetActive(false);
        BGMode.SetActive(true);
        
        GameObject waveObj = GameObject.Find("WaveSpawnManager");
        if (waveObj != null)
        {
            waveManager = waveObj.GetComponent<WaveManager>();
        }
        else
        {
            Debug.LogError("❌ WaveManager not found in scene!");
        }

        titleScreen.SetActive(true); 
    }

    void StartGame(int difficulty)
    {
        difficultyLevel = difficulty;

        
        if (waveManager != null)
        {
            waveManager.difficultyLevel = difficulty;
            waveManager.StartWaves();
            BGMode.SetActive(false);
            Player.SetActive(true);
            HUDGame.SetActive(true);
        }

        titleScreen.SetActive(false);
        
        Debug.Log("🎮 Start Game at difficulty: " + difficulty);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
    }
    public void ResetGame()
    {
        SceneManager.LoadScene("LevelGame");
        Time.timeScale = 1f;
    }
}