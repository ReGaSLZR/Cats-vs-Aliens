namespace ReGaSLZR.Base
{
    
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneLogic : MonoBehaviour
    {

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            return;
#endif
            Application.Quit();
        }

    }

}