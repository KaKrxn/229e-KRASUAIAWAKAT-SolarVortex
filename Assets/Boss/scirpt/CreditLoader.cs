using UnityEngine.SceneManagement;
using UnityEngine;

namespace Game.SceneManagement
{
    public class CreditLoader : MonoBehaviour
    {
        public void LoadGameScene()
        {
            SceneManager.LoadScene("Credits");
        }
    }
}