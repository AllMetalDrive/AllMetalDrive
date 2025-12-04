/*******************************************************
* Project: All Metal Drive
* Script: PlayerHealthFeedback.cs
* Author: José Cruz
* Created: 16/11/2025
* Last Modified: 17/11/2025 by José Cruz
*
* Description:
* Maneja el feedback visual y sonoro del sistema de salud
* del jugador. Utiliza el AudioManager para reproducir
* sonidos centralizados y genera efectos visuales al
* recibir daño, curarse o morir.
*
* Hours Worked: [1.5]
*
* Dependencies:
* - PlayerHealth.cs
* - AudioManager.cs
* - Prefabs de efectos visuales
*
* Sections:
* - VARIABLES
* - MÉTODOS PRINCIPALES
* - FUNCIONES AUXILIARES
* - EVENTOS UNITY
*
* Notes / Warnings:
* - Asegurarse de asignar los efectos visuales requeridos.
* - Los sonidos se gestionan únicamente desde AudioManager.
* - Mantiene la misma estructura que EnemyHealthFeedback.cs.
*   Considerar implementar una clase base común si aumenta
*   la repetición de código.
*******************************************************/

using UnityEngine;

public class PlayerHealthFeedback : MonoBehaviour
{
    // ==================================================
    // ===================== VARIABLES ===================
    // ==================================================

    [Header("REFERENCIAS")]
    [SerializeField] private PlayerHealth playerHealth;     // Referencia al sistema de salud del jugador
    [SerializeField] private AudioManager audioManager;     // Gestor centralizado de audio

    [Header("EFECTOS VISUALES")]
    [SerializeField] private GameObject damageEffect;       // Efecto al recibir daño
    [SerializeField] private GameObject healEffect;         // Efecto al recibir curación
    [SerializeField] private GameObject deathEffect;        // Efecto al morir

    [Header("CONFIGURACIÓN DE EFECTOS")]
    [SerializeField] private float effectLifetime = 2f;     // Tiempo antes de destruir un efecto visual



    // ==================================================
    // =============== MÉTODOS PRINCIPALES ==============
    // ==================================================

    /// <summary>
    /// Feedback al recibir daño: reproduce sonido y efecto visual.
    /// </summary>
    public void OnDamageFeedback()
    {
        audioManager?.PlayerTakeDamage();
        //TriggerEffect(damageEffect);  // Activar si se desea usar efectos visuales
    }

    /// <summary>
    /// Feedback al recibir curación.
    /// </summary>
    public void OnHealFeedback()
    {
        audioManager?.PlayerHeal();
        //TriggerEffect(healEffect);
    }

    /// <summary>
    /// Feedback al morir.
    /// </summary>
    public void OnDeathFeedback()
    {
        audioManager?.PlayerDeath();
        //TriggerEffect(deathEffect);
    }



    // ==================================================
    // ============= FUNCIONES AUXILIARES ===============
    // ==================================================

    /// <summary>
    /// Instancia un efecto visual y lo destruye tras un tiempo.
    /// </summary>
    private void TriggerEffect(GameObject effectPrefab)
    {
        if (effectPrefab == null) return;

        GameObject instance = Instantiate(
            effectPrefab,
            transform.position,
            Quaternion.identity
        );

        if (effectLifetime > 0f)
            Destroy(instance, effectLifetime);
    }



    // ==================================================
    // =================== EVENTOS UNITY ================
    // ==================================================

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = GetComponent<PlayerHealth>();

        if (audioManager == null)
            audioManager = FindAnyObjectByType<AudioManager>();
    }
}
