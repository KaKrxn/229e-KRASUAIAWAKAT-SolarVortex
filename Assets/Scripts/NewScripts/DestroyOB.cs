using Unity.VisualScripting;
using UnityEngine;
using UnityRandom = UnityEngine.Random;
using SysRandom = System.Random;
using System;

public class DestroyOB : MonoBehaviour
{
    public float EndPointZ;
    public float TimeDestroy;
    public int damage = 10;
    public int Point = 0;
    public  event Action<GameObject> OnDestroyEvent;
    PYController RefPlayer;

    //private PYController PYController;

    //public ParticleSystem[] explosionParticle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefPlayer = GameObject.Find("Player").GetComponent<PYController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Destroy(gameObject,TimeDestroy);

        if (transform.position.z < EndPointZ) 
        {
            Destroy(this.gameObject);
        }
    }
    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         PYController playerHealth = collision.gameObject.GetComponent<PYController>();
    //         if (playerHealth != null)
    //         {
    //             playerHealth.TakeDamage(damage); // สร้างดาเมจให้ Player
    //         }
    //         Destroy(gameObject); // ทำลายกระสุนเมื่อโดนเป้าหมาย
    //         //explosionParticle[0 - 6].Play();
    //     }
    // }

    void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject, 1f);
        PYController playerHealth = other.gameObject.GetComponent<PYController>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // สร้างดาเมจให้ Player
            }

        if (other.CompareTag("Missile"))
        {
           int idx = UnityRandom.Range(0, Point);
           RefPlayer.UpdateScore(idx);
            
        }
    }

     void OnDestroy()
    {
        if (OnDestroyEvent != null)
        {
            OnDestroyEvent(gameObject); // เรียก event และส่ง GameObject กลับไป
        }
    }
}
