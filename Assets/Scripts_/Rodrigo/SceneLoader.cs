using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("[SceneLoader] Se intentó cargar una escena sin nombre.");
            return;
        }
        SceneManager.LoadScene(sceneName);
    }
}
