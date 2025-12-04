/*******************************************************
* Project: [All Metal Drive]
* Script: PlayerHealth.cs
* Author: José Cruz
* Created: 15/11/2025
* Last Modified: 16/11/2025 by José Cruz
*
* Description:
* Implementación específica de HealthBase para el jugador.
* Controla la salud, muerte y cualquier efecto adicional
* relacionado con el estado del jugador.
*
* Hours Worked: [1]
*
* Dependencies:
* - HealthBase (clase padre)
* - UI de salud
* - Sistemas de daño (Projectile, MeleeAttack, etc.)
*
* Sections:
* - EVENTOS UNITY
* - MÉTODOS PRINCIPALES
*
* Notes / Warnings:
* - Eliminación del jugador (Destroy) está desactivada 
*   temporalmente para pruebas.
*******************************************************/

using UnityEngine;

public class PlayerHealth : HealthBase
{
    // ==================================================
    // ===================== VARIABLES ===================
    // ==================================================

    [Header("REFERENCIAS")]

    [SerializeField] private PlayerHealthFeedback playerHealthFeedback;  // Referencia al sistema de feedback de salud
    [SerializeField] private AudioManager audioManager;    // Referencia al AudioManager para reproducir sonidos

    // ==================================================
    // =================== EVENTOS UNITY ================
    // ==================================================

    /// <summary>
    /// Inicializa la salud del jugador basándose en la 
    /// configuración del HealthBase.
    /// </summary>
    protected override void Start()
    {
        base.Start();
    }


    // ==================================================
    // =============== MÉTODOS PRINCIPALES ==============
    // ==================================================

    /// <summary>
    /// Aplica daño al jugador y reproduce efectos de sonido
    /// cuando recibe daño.
    /// </summary>
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        // Activar feedback de daño (Audios y efectos visuales)
        playerHealthFeedback.OnDamageFeedback();
    }

    /// <summary>
    /// Cura al jugador y reproduce efectos de sonido
    /// cuando recibe curación.
    /// </summary>
    public override void Heal(float amount) // Al momento no hay curación en el juego, pero se deja preparado.
    {
        base.Heal(amount);

        // Activar feedback de curación (Audios y efectos visuales)
        playerHealthFeedback.OnHealFeedback();
    }

    /// <summary>
    /// Maneja la muerte del jugador. Aquí se debe incluir
    /// lógica de reinicio, animaciones o pantallas de game over.
    /// </summary>
    protected override void HandleDeath()
    {
        Debug.Log($"{gameObject.name} ha muerto.");

        // Activar feedback de muerte (Audios y efectos visuales)
        playerHealthFeedback.OnDeathFeedback();
        GameManagerUpdated.Instance.TriggerGameOver();
        

        // TODO: Activar animación de muerte
        // TODO: Desactivar controles del jugador

        //Destroy(gameObject);
    }
}
