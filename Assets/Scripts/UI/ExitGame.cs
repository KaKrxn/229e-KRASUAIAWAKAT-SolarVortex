using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // หยุดเกมใน Unity Editor
#else
            Application.Quit(); // ออกจากเกมเมื่อรันเป็น Build
#endif
    }
}