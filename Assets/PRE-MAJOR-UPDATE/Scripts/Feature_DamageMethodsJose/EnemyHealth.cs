/*******************************************************
* Project: [All Metal Drive]
* Script: EnemyHealth.cs
* Author: José Cruz
* Created: 15/11/2025
* Last Modified: 16/11/2025 by José Cruz
*
* Description:
* Implementación específica de HealthBase para enemigos.
* Controla la muerte e inicialización personalizada para 
* entidades enemigas.
*
* Hours Worked: [1]
*
* Dependencies:
* - HealthBase (clase padre)
* - Sistemas de daño (Projectile, MeleeAttack, etc.)
*
* Sections:
* - EVENTOS UNITY
* - MÉTODOS PRINCIPALES
*
* Notes / Warnings:
* - Eliminación del enemigo (Destroy) está desactivada 
*   temporalmente para pruebas.
*******************************************************/

using UnityEngine;

public class EnemyHealth : HealthBase
{
    // ==================================================
    // ===================== VARIABLES ===================
    // ==================================================

    [Header("REFERENCIAS")]
    [SerializeField] private EnemyHealthFeedback enemyHealthFeedback;


    // ==================================================
    // =================== EVENTOS UNITY ================
    // ==================================================

    /// <summary>
    /// Inicializa la salud base del enemigo.
    /// </summary>
    protected override void Start()
    {
        base.Start();
    }


    // ==================================================
    // =============== MÉTODOS PRINCIPALES ==============
    // ==================================================


    /// <summary>
    /// Aplica daño al enemigo y reproduce efectos de sonido
    /// cuando recibe daño.
    /// </summary>
    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);

        // Activar feedback de daño (Audios y efectos visuales)
        enemyHealthFeedback.OnDamageFeedback();
    }

    /// <summary>
    /// Cura al enemigo y reproduce efectos de sonido
    /// cuando recibe curación.
    /// </summary>
    public override void Heal(int amount) // Al momento no hay curación en el juego, pero se deja preparado.
    {
        base.Heal(amount);

        // Activar feedback de curación (Audios y efectos visuales)
        enemyHealthFeedback.OnHealFeedback();
    }

    /// <summary>
    /// Maneja la muerte del enemigo cuando su salud llega a cero.
    /// </summary>
    protected override void HandleDeath()
    {
        Debug.Log($"{gameObject.name} ha muerto.");

        // Activar feedback de muerte (Audios y efectos visuales)
        enemyHealthFeedback.OnDeathFeedback();

        // TODO: Implementar animación de muerte
        // TODO: Soltar ítems o dar recompensas al jugador
        // TODO: Desactivar componentes del enemigo

        //Destroy(gameObject);
    }
}
