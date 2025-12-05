using UnityEngine;

public class UIScreenManager : MonoBehaviour
{
    [Header("Pantallas UI")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject victoryScreen;

    // Llama este método para mostrar la pantalla de Game Over
    public void ShowGameOver()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);
        else
            Debug.LogWarning("UIScreenManager: gameOverScreen no asignado.");
    }

    public void ShowVictoryScreen()
    {
        if (victoryScreen != null)
            victoryScreen.SetActive(true);
        else
            Debug.LogWarning("UIScreenManager: victoryScreen no asignado.");
    }
}
