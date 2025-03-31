using UnityEngine.SceneManagement;
using UnityEngine;

namespace Game.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadGameScene()
        {
            SceneManager.LoadScene("Credits");
        }
    }
}