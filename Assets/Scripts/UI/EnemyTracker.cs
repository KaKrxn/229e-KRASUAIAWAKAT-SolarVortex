using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTracker : MonoBehaviour
{
    public RectTransform trackingFrame; // กรอบสีเขียว
    public Image targetIndicator;       // วงกลม Track
    public Camera mainCamera;
    public LayerMask enemyLayer;
    public float trackingRange = 100f;
    public float lockOnTime = 1.5f;
    
    private Transform lockedTarget;
    private float lockOnProgress = 0f;
    private List<Transform> enemies = new List<Transform>();

    void Update()
    {
        FindEnemies();
        UpdateTrackingUI();
        HandleLockOn();
    }

    void FindEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, trackingRange, enemyLayer);
        enemies.Clear();
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                enemies.Add(hit.transform);
            }
        }
    }

    void UpdateTrackingUI()
    {
        if (lockedTarget == null) return;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(lockedTarget.position);
        if (screenPos.z > 0 && screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height)
        {
            targetIndicator.enabled = true;
            targetIndicator.rectTransform.position = screenPos;
        }
        else
        {
            targetIndicator.enabled = false;
        }
    }

    void HandleLockOn()
    {
        if (enemies.Count == 0) return;
        
        if (lockedTarget == null)
        {
            lockedTarget = enemies[0];
            lockOnProgress = 0f;
        }
        else
        {
            lockOnProgress += Time.deltaTime;
            if (lockOnProgress >= lockOnTime)
            {
                LockOnTarget();
            }
        }
    }

    void LockOnTarget()
    {
        Debug.Log("Locked on to : " + lockedTarget.name);
    }

    public Transform GetLockedTarget()
    {
        return lockedTarget;
    }
}