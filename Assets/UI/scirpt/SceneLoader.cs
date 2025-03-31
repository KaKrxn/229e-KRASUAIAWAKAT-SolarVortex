using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Boss"); // เปลี่ยนเป็นชื่อซีนของคุณ
    }
}
