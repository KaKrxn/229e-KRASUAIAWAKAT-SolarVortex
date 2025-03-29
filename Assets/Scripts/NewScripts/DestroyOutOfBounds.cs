using UnityEngine;
using System;

public class DestroyOutOfBounds : MonoBehaviour
{
    public  event Action<GameObject> OnDestroyEvent; 

    void Update()
    {
        if (transform.position.y < -10f) 
        {
            OnDestroyEvent?.Invoke(gameObject); 
            //Destroy(gameObject);
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
