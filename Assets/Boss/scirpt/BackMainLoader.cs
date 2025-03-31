using UnityEngine.SceneManagement;
using UnityEngine;

namespace Game.SceneManagement
{
    public class BackMainLoader : MonoBehaviour
    {
        public void LoadGameScene()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}