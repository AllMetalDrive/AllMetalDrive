/*******************************************************
* Project: All Metal Drive
* Script: GameManager.cs
* Author: Rodrigo Garcia de Quevedo Contreras
* Created: 18/11/2025
* Last Modified: 27/11/2025 by Rodrigo Garcia de Quevedo Contreras
*
* Description:
* Administrador central del juego que maneja los estados del juego
* Maneja el cambio entre escenas
* Maneja la pausa y reanudación del juego
* Controla las transiciones entre menús, niveles y escenas de victoria/derrota
* Flujo: MainMenu → Bosses → Lobby → Victory/GameOver.
*
* Hours Worked: 2h 30m
*
* Dependencies:
* - UnityEngine.SceneManagement
*
* Sections:
* - VARIABLES ([Header()])
* - MÉTODOS PRINCIPALES
* - FUNCIONES AUXILIARES
* - EVENTOS UNITY
*
* Notes / Warnings:
* - El GameManager debe existir solo una vez.
* - El GameObject debe permanecer entre escenas.
* - Asegúrate de que los nombres de las escenas coincidan con los definidos en el enum GameState.
* - El tiempo de juego se detiene al pausar (Time.timeScale = 0).
* - El GameManager se inicializa en Awake() para evitar duplicados.
* - La escena inicial puede ser configurada en el inspector.
* - El estado previo se guarda al pausar para reanudar correctamente.
* - Cambiar de estado carga la escena correspondiente automáticamente.
* - El GameManager no fuerza automáticamente VictoryScene tras Boss3.
*******************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // ==================================================
    // ================ VARIABLES HEADER ================
    // ==================================================

    // GameState describe los estados del juego y sus escenas. Está anidado en GameManager para encapsular su uso. 
    // Si otros scripts deben acceder a él, se referencia como `GameManager.GameState`.
    public enum GameState
    {
        MainMenu,
        VictoryScene,
        GameOverScene,
        Pause,
        Boss1,
        Boss2,
        Boss3,
        Lobby
    }

    [Header("ESTADO DEL JUEGO")]
    // Estado actual y previo del juego
    [SerializeField] private GameState currentState = GameState.MainMenu;
    private GameState previousState;

    // Indica si el juego está pausado
    private bool _isPaused = false;

    [Header("CONFIGURACIÓN DE ESCENAS")]
    [SerializeField] private string initialSceneName = "";

    [Header("UI DE PAUSA (ASIGNAR PANEL)")]
    [SerializeField] private GameObject pauseMenuUI;

    // Instancia única del GameManager
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    // ==================================================
    // ================== EVENTOS UNITY =================
    // ==================================================

    private void Awake()
    {
        //Evita duplicados del GameManager
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Cargar escena inicial si está asignada
        if (!string.IsNullOrEmpty(initialSceneName))
            LoadScene(initialSceneName);

        // Ocultar el menú al iniciar
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        // Detectar ESC para pausar
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // ==================================================
    // ================ MÉTODOS PRINCIPALES =============
    // ==================================================

    /// <summary>
    /// Cambia el estado actual del juego y ejecuta la transición correspondiente.
    /// </summary>
    public void ChangeState(GameState newState)
    {
        // Si otro script llama ChangeState(Pause), tratamos esto como un toggle.
        if (newState == GameState.Pause)
        {
            TogglePause();
            return;
        }

        currentState = newState;

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

            case GameState.Boss1:
                LoadScene("Boss1");
                ResumeGame();
                break;

            case GameState.Boss2:
                LoadScene("Boss2");
                ResumeGame();
                break;

            case GameState.Boss3:
                LoadScene("Boss3");
                ResumeGame();
                break;

            case GameState.VictoryScene:
                LoadScene("VictoryScene");
                ResumeGame();
                break;

            case GameState.GameOverScene:
                LoadScene("GameOverScene");
                ResumeGame();
                break;
        }
    }

    /// <summary>
    /// Carga una escena por nombre utilizando el SceneManager.
    /// </summary>
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

    /// <summary>
    /// Pausa el juego y guarda el estado previo.
    /// </summary>
    public void PauseGame()
    {
        if (_isPaused) return;

        previousState = currentState;
        _isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
    }

    /// <summary>
    /// Reanuda el juego devolviendo el flujo al estado previo.
    /// </summary>
    public void ResumeGame()
    {
        if (!_isPaused) return;

        _isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        // Restaura correctamente el estado previo
        currentState = previousState;
    }

    /// <summary>
    /// Alterna entre pausar y reanudar el juego.
    /// </summary>
    public void TogglePause()
    {
        if (_isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    /// <summary>
    /// Llama la escena de Game Over.
    /// </summary>
    public void TriggerGameOver()
    {
        ChangeState(GameState.GameOverScene);
    }

    /// <summary>
    /// Llama la escena de Victoria.
    /// </summary>
    public void TriggerVictory()
    {
        ChangeState(GameState.VictoryScene);
    }

    /// <summary>
    /// Regresa si el juego está actualmente pausado.
    /// </summary>
    public bool IsPaused()
    {
        return _isPaused;
    }
}
