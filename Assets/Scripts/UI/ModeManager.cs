using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    [Header("Enemy Elements")]
    public float EnemyHP;
    public float EnemyATK;
    public float EnemySpeed;
    public float EnemyFireDelay;
    public float EnemyBulletSpeed;

    [Header("Asteroid Elements")]
    public float MaxSpeedAsteroid;

    void Awake()
    {
        easyButton.onClick.AddListener(() => {StartGame(1.0f);});
        mediumButton.onClick.AddListener(() => {StartGame(1.5f);});
        hardButton.onClick.AddListener(() => {StartGame(2.0f);});
    }
    

    void StartGame(float diffculty)
    {
        //spawnRate /= diffculty;
        //titleScreen.SetActive(false);
        //StartCoroutine(SpawnTargets());
    }
}
