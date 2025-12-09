using UnityEngine;

public class GameplaySceneInitializer : MonoBehaviour
{
    void Start()
    {
        // Cambia el estado del GameManager a Gameplay al iniciar la escena Boss1
        if (GameManagerUpdated.Instance != null)
        {
            GameManagerUpdated.Instance.ChangeState(GameManagerUpdated.GameState.Gameplay);
        }
    }
}
