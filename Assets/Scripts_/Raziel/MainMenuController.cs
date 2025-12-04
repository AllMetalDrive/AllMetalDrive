/*******************************************************
* Project: All Metal Drive
* Script: MainMenuController.cs
* Author: Raziel
* Created: 18/11/2025
* Last Modified: 21/11/2025 by Raziel
*
* Description:
* Handles the main menu interactions for the game.
* Manages Play, Settings, and Exit buttons.
* Loads Scene 1 when Play is pressed.
* Includes optional button click sound.
* Opens and closes the Settings Panel.
*
* Hours Worked: 1h 10m
*
* Dependencies:
* - UnityEngine.SceneManagement
* - UI Buttons assigned through the Inspector
*
* Sections:
* - VARIABLES ([Header()])
* - MAIN METHODS
* - AUXILIARY FUNCTIONS
*
* Notes / Warnings:
* - Animations not implemented yet.
* - Button highlight visuals are placeholders.
*******************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // ==================================================
    // ================== VARIABLES =====================
    // ==================================================

    [Header("BUTTON REFERENCES")]
    [SerializeField] private UnityEngine.UI.Button playButton;
    [SerializeField] private UnityEngine.UI.Button settingsButton;
    [SerializeField] private UnityEngine.UI.Button exitButton;

    [Header("SETTINGS PANEL")]
    // The full screen panel for settings
    [SerializeField] private GameObject settingsPanel;

    // The back button inside the settings panel
    [SerializeField] private UnityEngine.UI.Button backButton;

    [Header("AUDIO")]
    [SerializeField] private AudioSource buttonClickSfx;

    [Header("DEBUG")]
    [SerializeField] private bool debugMode = false;



    // ==================================================
    // ================= MAIN METHODS ===================
    // ==================================================

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayPressed);
        settingsButton.onClick.AddListener(OnSettingsPressed);
        exitButton.onClick.AddListener(OnExitPressed);

        backButton.onClick.AddListener(OnBackPressed);

        settingsPanel.SetActive(false);

        if (debugMode) Debug.Log("[MainMenu] Main menu initialized.");
    }



    // ==================================================
    // ================ AUXILIARY FUNCTIONS =============
    // ==================================================

    /// <summary>
    /// Loads scene 1 (gameplay).
    /// </summary>
    private void OnPlayPressed()
    {
        PlaySfx();
        if (debugMode) Debug.Log("[MainMenu] Loading Scene 1...");
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Opens the settings panel.
    /// </summary>
    private void OnSettingsPressed()
    {
        PlaySfx();

        if (debugMode) Debug.Log("[MainMenu] Opening settings panel.");

        settingsPanel.SetActive(true);
    }

    /// <summary>
    /// Closes the settings panel.
    /// </summary>
    private void OnBackPressed()
    {
        PlaySfx();

        if (debugMode) Debug.Log("[MainMenu] Closing settings panel.");

        settingsPanel.SetActive(false);
    }

    /// <summary>
    /// Exits the application (works in Editor and Build).
    /// </summary>
    private void OnExitPressed()
    {
        PlaySfx();

        if (debugMode) Debug.Log("[MainMenu] Quitting game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Plays button click SFX.
    /// </summary>
    private void PlaySfx()
    {
        if (buttonClickSfx != null)
            buttonClickSfx.Play();
    }
}