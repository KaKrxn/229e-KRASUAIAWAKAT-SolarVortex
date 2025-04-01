using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class PYController : MonoBehaviour
{
    public GameObject Player;
    public int maxHP = 100;
    private int currentHP;
    public float MoveSpeed;
    public float XRange;
    public float YRange;

    [Header("Missile System")]
    public Transform firePoint;
    public GameObject missilePrefab;
    public float missileCooldown = 3f;
    private bool canShoot = true;
    private Transform lockedTarget;

    private InputAction moveAction;
    private InputAction shootAction;
    private InputAction lockTargetAction;
    private InputAction SwitchCam;
    private float horizontalInput;
    private float verticalInput;

    [Header("Text")]
    public TextMeshProUGUI scoreText;
    private int score;
    public Slider HealthBar;

    public ParticleSystem explosionParticle;
    [Header("Camera")]
    public GameObject HUD;
    public GameObject GameOver;
    public GameObject cameraRef;
    public AudioClip crashSfx;
    private AudioSource playerAudio;
    private int countPress = 0;
    private bool isCameraOn = true;
    private bool isHUDOn = false;
    private bool isCanShoot = false;

    [SerializeField] Rigidbody RB;

    private void Awake()
    {
        currentHP = maxHP;
        HealthBar.maxValue = maxHP;
        HealthBar.value = currentHP;
        moveAction = InputSystem.actions.FindAction("Move");
        shootAction = InputSystem.actions.FindAction("Shoot");
        lockTargetAction = InputSystem.actions.FindAction("LockTarget");
        SwitchCam = InputSystem.actions.FindAction("SwitchCam");
        playerAudio = GetComponent<AudioSource>();

        isCanShoot = false;
        isHUDOn = false;
        GameOver.SetActive(false);

        RB = GetComponent<Rigidbody>();
        //cameraRef = GameObject.Find("CameraPlayer").GetComponent<CameraFollowerV2>();
    }

    private void OnEnable()
    {
        shootAction.started += ShootMissile;
        lockTargetAction.started += LockTarget;
        SwitchCam.Enable();
    }

    private void OnDisable()
    {
        shootAction.started -= ShootMissile;
        lockTargetAction.started -= LockTarget;
        SwitchCam.Disable();
    }

    void Update()
    {
        if (SwitchCam.triggered) 
        {
            isCameraOn = !isCameraOn; 
            isHUDOn = !isHUDOn;
            isCanShoot = !isCanShoot;
            cameraRef.SetActive(isCameraOn); 
            HUD.SetActive(isHUDOn);
            Debug.Log(isCameraOn ? "Open Camera" : "Close Camera");
        }
    }

    void FixedUpdate()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        horizontalInput = moveInput.x;
        verticalInput = moveInput.y;

        RB.AddForce(horizontalInput * MoveSpeed * Time.deltaTime * Vector3.right);
        RB.AddForce(verticalInput * MoveSpeed * Time.deltaTime * Vector3.down);

        
        
        RollShip();  
        PitchShip(); 
        FixYRotation(); 
        ClampPosition(); 

    }

    
    void RollShip()
    {
        float targetXRotation = horizontalInput * 30f;
        Vector3 currentRotation = transform.rotation.eulerAngles;
        float currentX = currentRotation.x;
        if (currentX > 180) currentX -= 360;
        float newX = Mathf.Lerp(currentX, targetXRotation, Time.deltaTime * 5f);
        transform.rotation = Quaternion.Euler(newX, currentRotation.y, currentRotation.z);
    }

   
    void PitchShip()
    {
        
        float targetZRotation = verticalInput * 25f;  
        Vector3 currentRotation = transform.rotation.eulerAngles;
        float currentZ = currentRotation.z;
        if (currentZ > 180f) currentZ -= 360f;  
        float newZ = Mathf.Lerp(currentZ, targetZRotation, Time.deltaTime * 5f);  
        transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, newZ);
    }

    // ฟังก์ชันการควบคุมตำแหน่งของเครื่องบิน
    void ClampPosition()
    {
        if (transform.position.x < -XRange)
        {
            transform.position = new Vector3(-XRange, transform.position.y, transform.position.z);
            RB.linearVelocity = new Vector3(0, RB.linearVelocity.y, RB.linearVelocity.z);
        }
        if (transform.position.x > XRange)
        {
            transform.position = new Vector3(XRange, transform.position.y, transform.position.z);
            RB.linearVelocity = new Vector3(0, RB.linearVelocity.y, RB.linearVelocity.z);
        }

        if (transform.position.y < -YRange)
        {
            transform.position = new Vector3(transform.position.x, -YRange, transform.position.z);
            RB.linearVelocity = new Vector3(RB.linearVelocity.x, 0, RB.linearVelocity.z);
        }
        if (transform.position.y > YRange)
        {
            transform.position = new Vector3(transform.position.x, YRange, transform.position.z);
            RB.linearVelocity = new Vector3(RB.linearVelocity.x, 0, RB.linearVelocity.z);
        }
    }

    private void LockTarget(InputAction.CallbackContext context)
    {
        if (Camera.main == null)
    {
        Debug.LogError("❌ No camera with 'MainCamera' tag found in the scene!");
        return;
    }
        
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                lockedTarget = hit.transform;
                Debug.Log("✅ Target Locked: " + lockedTarget.name);
            }
        }

    // Debug ช่วยดูว่า Ray ยิงไปทางไหน
    Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);
    }

    private void ShootMissile(InputAction.CallbackContext context)
    {
        
        if (!canShoot) return;

    
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        
        if (closestEnemy != null)
        {
            lockedTarget = closestEnemy;
            Debug.Log("🎯 Auto-Locked onto: " + lockedTarget.name);
        }
        else
        {
            Debug.LogWarning("❌ No enemy found to lock on.");
            return; 
        }

        
        
        
        GameObject missile = Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
        MissileTracking missileScript = missile.GetComponent<MissileTracking>();
        missileScript.SetTarget(lockedTarget);
        
        

        Debug.Log("🚀 Missile launched toward: " + lockedTarget.name);
        StartCoroutine(MissileCooldown());
    }

    IEnumerator MissileCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(missileCooldown);
        canShoot = true;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        HealthBar.value = currentHP;
        Debug.Log("Player HP: " + currentHP);
        if (currentHP <= 0)
        {
            
            //explosionParticle.Play();
            Debug.Log("Player Died!");
            GameOver.SetActive(true);
            Time.timeScale = 0f;

            
              //StartCoroutine(HandleDeathAndLoadScene());
            
        }
    }

    void FixYRotation()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currentRotation.x, 90f, currentRotation.z);
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     RB.linearVelocity = Vector3.zero;
    //     Vector3 currentRotation = transform.rotation.eulerAngles;
    //     transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);
    //     Debug.Log("Hit Enemy, reset Force and Rotation.");
    //     // Destroy(this.gameObject);
    //     // Instantiate(Player, transform.position, Player.transform.rotation);
    // }
    public void UpdateScore(int score)
    {
        this.score += score;
        scoreText.text = this.score.ToString();
        playerAudio.PlayOneShot(crashSfx);

    }

    public void RestoreFullHP()
    {
        currentHP = maxHP;
        HealthBar.value = currentHP;
    }

    public void PlaySound()
    {
        playerAudio.PlayOneShot(crashSfx);

    }
    // IEnumerator HandleDeathAndLoadScene()
    // {
    //     // ✅ รอเวลาแบบไม่โดน Time.timeScale (ไม่งั้นจะไม่รอจริง)
    //     yield return new WaitForSecondsRealtime(2f);

    //     // ✅ รีเซ็ต Time.timeScale ก่อนโหลดฉากใหม่
    //     Time.timeScale = 1f;
    //     SceneManager.LoadScene("Main Menu");
    // }

    void SwitchCamera()
    {
        // if (SwitchCam.wasPressedThisFrame) // ใช้ wasPressedThisFrame เพื่อให้ทำงานครั้งเดียวต่อการกด
        // {
        //     countPress = 1 - countPress; // สลับค่า 0 ↔ 1
        //     cameraRef.SetActive(countPress == 1); // เปิด-ปิดกล้องตามค่า countPress

        //     Debug.Log(countPress == 1 ? "Open Camera" : "Close Camera");
        // }
    }

    






}



