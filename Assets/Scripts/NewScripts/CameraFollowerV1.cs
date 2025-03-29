using Unity.VisualScripting;
using UnityEngine;

public class CameraFollowerV1 : MonoBehaviour
{

    private CameraFollowerV2 cameraFollowerV2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraFollowerV2 = GetComponent<CameraFollowerV2>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            cameraFollowerV2.CameraOpen = true;
            cameraFollowerV2.gameObject.SetActive(true);
        }
    }
}
