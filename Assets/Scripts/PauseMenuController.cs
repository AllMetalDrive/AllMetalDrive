/*******************************************************
* Project: All Metal Drive
* Script: PauseMenuController.cs
* Author: Raziel
* Created: 18/11/2025
* Last Modified: 21/11/2025 by Raziel
*
* Description:
* Handles the pause menu logic. The game pauses when
* pressing ESC and displays the pause menu panel.
* Includes Resume, Settings (with simple panel), and
* return to Main Menu.
*
* Hours Worked: 1h 00m
*
* Dependencies:
* - UI Buttons assigned through the Inspector
* - SceneManager (UnityEngine.SceneManagement)
* - Uses the new Unity Input System (InputSystem)
*
* Sections:
* - VARIABLES ([Header()])
* - MAIN METHODS
* - AUXILIARY FUNCTIONS
*
* Notes / Warnings:
* - Settings menu contains only a Return button for now.
* - Time.timeScale is used to pause the game.
*******************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    // ==================================================
    // ================== VARIABLES =====================
    // ==================================================

    [Header("PAUSE MENU UI")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private UnityEngine.UI.Button resumeButton;
    [SerializeField] private UnityEngine.UI.Button settingsButton;
    [SerializeField] private UnityEngine.UI.Button mainMenuButton;

    [Header("SETTINGS MENU UI")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private UnityEngine.UI.Button returnButton;

    [Header("DEBUG")]
    [SerializeField] private bool debugMode = false;

    private bool _isPaused = false;



    // ==================================================
    // ================= MAIN METHODS ===================
    // ==================================================

    private void Start()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);

        resumeButton.onClick.AddListener(OnResumePressed);
        settingsButton.onClick.AddListener(OnSettingsPressed);
        mainMenuButton.onClick.AddListener(OnMainMenuPressed);

        returnButton.onClick.AddListener(OnReturnPressed);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (settingsPanel.activeSelf)
            {
                // Escape closes settings first
                OnReturnPressed();
                return;
            }

            if (_isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }



    // ==================================================
    // ================ AUXILIARY FUNCTIONS =============
    // ==================================================

    private void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);

        if (debugMode) Debug.Log("[PauseMenu] Game paused.");
    }

    private void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);

        if (debugMode) Debug.Log("[PauseMenu] Game resumed.");
    }

    private void OnResumePressed()
    {
        ResumeGame();
    }

    private void OnSettingsPressed()
    {
        if (debugMode) Debug.Log("[PauseMenu] Opening Settings panel...");

        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    private void OnReturnPressed()
    {
        if (debugMode) Debug.Log("[PauseMenu] Returning to Pause Menu...");

        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    private void OnMainMenuPressed()
    {
        if (debugMode) Debug.Log("[PauseMenu] Returning to Main Menu...");

        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}