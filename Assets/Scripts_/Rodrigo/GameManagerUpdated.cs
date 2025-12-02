using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManagerUpdated : MonoBehaviour
{
	// ==================================================
	// ================ VARIABLES HEADER ================
	// ==================================================

	public enum GameState
	{
		MainMenu,
		Gameplay,
		Pause,
		VictoryScene,
		GameOverScene,
		Lobby
	}

	[Header("ESTADO DEL JUEGO")]
	[SerializeField] private GameState currentState = GameState.MainMenu;
	public GameState CurrentState => currentState; // Propiedad pública de solo lectura
	private GameState previousState;
	private bool _isPaused = false;

	[Header("CONFIGURACIÓN DE ESCENAS")]
	[SerializeField] private string initialSceneName = "";

	private static GameManagerUpdated Intance;
	public static GameManagerUpdated Instance => Intance;

	[Header("MENÚS UI")]
	[SerializeField] private GameObject pauseMenuPanel; // Asigna el panel del menú de pausa en el inspector
	[SerializeField] private GameObject gameOverMenuPanel; // Asigna el panel del menú de Game Over en el inspector
    [SerializeField] private GameObject VictoryMenuPanel; // Asigna el panel del menú de Victoria en el inspector

	// ==================================================
	// ================== EVENTOS UNITY =================
	// ==================================================

	private void Awake()
	{
		if (Intance != null && Intance != this)
		{
			Destroy(gameObject);
			return;
		}
		Intance = this;
		//DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		if (!string.IsNullOrEmpty(initialSceneName))
			LoadScene(initialSceneName);
		if (pauseMenuPanel != null)
			pauseMenuPanel.SetActive(false);
		if (gameOverMenuPanel != null)
			gameOverMenuPanel.SetActive(false);
		previousState = currentState; // Inicializa el estado previo correctamente
	}

	[Header("TECLA DE PAUSA")]
	[SerializeField] private KeyCode pauseKey = KeyCode.Escape;

	private void Update()
	{
		if (Input.GetKeyDown(pauseKey) && currentState != GameState.MainMenu)
		{
			TogglePause();
		}
	}

	// ==================================================
	// ================ MÉTODOS PRINCIPALES =============
	// ==================================================

	public void ChangeState(GameState newState)
	{
		currentState = newState;

		// Oculta todos los menús por defecto
		if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
		if (gameOverMenuPanel != null) gameOverMenuPanel.SetActive(false);

		switch (newState)
		{
			case GameState.MainMenu:
				LoadScene("MainMenu");
				ResumeGame();
				break;
			case GameState.Lobby:
				LoadScene("Lobby");
				ResumeGame();
				break;
			case GameState.Gameplay:
				// Aquí puedes cargar la escena de gameplay si lo deseas
				ResumeGame();
				break;
			case GameState.VictoryScene:
				if (VictoryMenuPanel != null)
					VictoryMenuPanel.SetActive(true);
				ResumeGame();
				break;
			case GameState.GameOverScene:
				if (gameOverMenuPanel != null)
					gameOverMenuPanel.SetActive(true);
				break;
			case GameState.Pause:
				if (pauseMenuPanel != null)
					pauseMenuPanel.SetActive(true);
				break;
		}
	}

	public void LoadScene(string sceneName)
	{
		if (string.IsNullOrEmpty(sceneName))
		{
			Debug.LogWarning("[GameManager] Se intentó cargar una escena sin nombre.");
			return;
		}
		SceneManager.LoadScene(sceneName);
	}

	// ==================================================
	// ============== FUNCIONES AUXILIARES =============
	// ==================================================

	/* public void PauseGame()
	{
		if (_isPaused) return;
		// Solo guarda el estado si no es Pause ni GameOverScene
		if (currentState != GameState.Pause && currentState != GameState.GameOverScene)
			previousState = currentState;
		_isPaused = true;
		Time.timeScale = 0f;
	} */

	public void ResumeGame()
	{
		if (!_isPaused) return;
		_isPaused = false;
		Time.timeScale = 1f;
		if (pauseMenuPanel != null)
			pauseMenuPanel.SetActive(false);
	}

	public void TogglePause()
	{
		if (_isPaused)
		{
			ResumeGame();
			// Solo regresa al estado previo si es válido
			if (previousState != GameState.Pause && previousState != GameState.GameOverScene)
				ChangeState(previousState);
		}
		else
		{
			ChangeState(GameState.Pause);
		}
	}

	public void TriggerGameOver()
	{
		ChangeState(GameState.GameOverScene);
	}

	public void TriggerVictory()
	{
		ChangeState(GameState.VictoryScene);
	}
}
