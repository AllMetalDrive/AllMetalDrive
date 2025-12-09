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
		GameOverScreen,
		Lobby
	}

	[Header("ESTADO DEL JUEGO")]
	[SerializeField] private GameState currentState = GameState.MainMenu;
	public GameState CurrentState => currentState; // Propiedad pública de solo lectura
	private GameState previousState;
	private bool _isPaused = false;

	[Header("CONFIGURACIÓN DE ESCENAS")]
	[SerializeField] private string initialSceneName = "";

	private static GameManagerUpdated _instance;
	public static GameManagerUpdated Instance => _instance;

	[Header("MENÚS UI")]
	[SerializeField] private GameObject pauseMenuPanel; // Asigna el panel del menú de pausa en el inspector

	[Header("REFERENCIAS")]
	[SerializeField] private UIScreenManager uiScreenManager; // Referencia al manejador de pantallas UI

	// ==================================================
	// ================== EVENTOS UNITY =================
	// ==================================================

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
			return;
		}
		_instance = this;
		//DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		if (!string.IsNullOrEmpty(initialSceneName))
			//LoadScene(initialSceneName);
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

		// if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);

		switch (newState)
		{


			case GameState.MainMenu:
				/* _isPaused = false;
				Time.timeScale = 1f; */
				// if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
				ResumeGame();
				break;
			case GameState.Lobby:
				/* _isPaused = false;
				Time.timeScale = 1f; */
				// if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
				break;
			case GameState.Gameplay:
				/* _isPaused = false;
				Time.timeScale = 1f; */
				// if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
				ResumeGame();
				break;
			case GameState.VictoryScene:
				/* _isPaused = false;
				Time.timeScale = 1f; */
				ResumeGame();
				uiScreenManager.ShowVictoryScreen();
				// if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
				break;
			case GameState.GameOverScreen:
				/* _isPaused = false;
				Time.timeScale = 1f; */
				ResumeGame();
				// if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
				break;
			case GameState.Pause:
				_isPaused = true;
				Time.timeScale = 0f;
				if (pauseMenuPanel != null)
					pauseMenuPanel.SetActive(true);
				break;
		}
	}

	/* public void LoadScene(string sceneName)
	{
		if (string.IsNullOrEmpty(sceneName))
		{
			Debug.LogWarning("[GameManager] Se intentó cargar una escena sin nombre.");
			return;
		}
		SceneManager.LoadScene(sceneName);
	} */

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
			// Regresa al estado previo solo si era jugable
			if (previousState == GameState.Gameplay || previousState == GameState.Lobby)
			{
				ChangeState(previousState);
			}
		}
		else
		{
			// Guarda el estado previo solo si el estado actual es jugable
			if (currentState == GameState.Gameplay || currentState == GameState.Lobby)
			{
				previousState = currentState;
			}
			ChangeState(GameState.Pause);
		}
	}

	public void TriggerGameOver()
	{
		ChangeState(GameState.GameOverScreen);

		// Mostrar pantalla de Game Over
		if (uiScreenManager != null)
		{
			uiScreenManager.ShowGameOverScreen();
		}
		else
		{
			Debug.LogWarning("UIScreenManager no asignado.");
		}
	}

	public void TriggerVictory()
	{
		ChangeState(GameState.VictoryScene);
		Debug.Log("¡Victoria! Has derrotado al boss.");
	}
}
