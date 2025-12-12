/*******************************************************
* Project: [All Metal Drive]
* Script: HealthUI.cs
* Author: Rodrigo Garcia de Quevedo
* Created: 08/12/2025
* Last Modified: 10/12/2025
*
* Description:
* Controlador universal de UI para salud del jugador o enemigo.
* Escucha eventos del sistema de salud (HealthBase) y actualiza la barra de vida en tiempo real al recibir daño, curación o muerte.
* Puede usarse para PlayerHealth o EnemyHealth.
*
* Hours Worked: [1.5]
*
* Dependencies:
* - HealthBase / PlayerHealth / EnemyHealth
* - Slider o Image type Filled
* - Canvas UI del juego
*
* Sections:
* - VARIABLES
* - EVENTOS UNITY
* - MÉTODOS PRINCIPALES
* - FUNCIONES AUXILIARES
*
* Notes / Warnings:
* - Debe ser asignado a un elemento UI con Slider o Filled Image.
* - TargetHealth debe apuntar a un objeto en escena con HealthBase.
* - Puede ocultarse automáticamente al morir si se desea.
*******************************************************/

using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    // ==================================================
    // ===================== VARIABLES ===================
    // ==================================================

    [Header("REFERENCIAS")]
    [Tooltip("Referencia al objeto con HealthBase (Player o Enemigo).")]
    [SerializeField] private HealthBase targetHealth;

    [Tooltip("Slider que representa la vida en UI.")]
    [SerializeField] private Slider healthSlider;

    [Header("CONFIGURACIÓN")]
    [Tooltip("Oculta la barra cuando está llena.")]
    [SerializeField] private bool hideWhenFull = false;

    [Tooltip("Oculta la barra al morir.")]
    [SerializeField] private bool hideOnDeath = true;



    // ==================================================
    // =================== EVENTOS UNITY ================
    // ==================================================

    private void Awake()
    {
        if (targetHealth == null)
        {
            Debug.LogWarning($"[HealthUI] No se asignó TargetHealth en {name}");
            return;
        }

        // Suscripción de eventos
        targetHealth.OnHealthChanged += UpdateHealthBar;
        targetHealth.OnDeathEvent += HandleDeathUI;
    }

    private void Start()
    {
        InitializeHealthUI();
    }



    // ==================================================
    // =============== MÉTODOS PRINCIPALES ==============
    // ==================================================

    /// <summary>
    /// Configura el UI con la vida inicial.
    /// </summary>
    private void InitializeHealthUI()
    {
        if (healthSlider == null || targetHealth == null) return;

        int current = GetCurrentHealth();
        int max = GetMaxHealth();

        healthSlider.maxValue = max;
        healthSlider.value = current;

        // Si está lleno y hideWhenFull = true → se oculta
        if (hideWhenFull)
            healthSlider.gameObject.SetActive(current < max);
    }

    /// <summary>
    /// Actualiza la barra cuando cambia la salud.
    /// </summary>
    private void UpdateHealthBar(int current, int max)
    {
        if (healthSlider == null) return;

        healthSlider.maxValue = max;
        healthSlider.value = current;

        if (hideWhenFull)
            healthSlider.gameObject.SetActive(current < max);
    }

    /// <summary>
    /// Reacción UI al morir la entidad.
    /// </summary>
    private void HandleDeathUI()
    {
        if (hideOnDeath && healthSlider != null)
            healthSlider.gameObject.SetActive(false);
    }



    // ==================================================
    // ============= FUNCIONES AUXILIARES ===============
    // ==================================================

    private int GetCurrentHealth()
    {
        return (int)typeof(HealthBase)
            .GetField("currentHealth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(targetHealth);
    }

    private int GetMaxHealth()
    {
        return (int)typeof(HealthBase)
            .GetField("maxHealth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(targetHealth);
    }
}
