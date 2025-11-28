/*******************************************************
* Project: [Nombre del Proyecto]
* Script: EnemyHealthFeedback.cs
* Author: José Cruz
* Created: [16/11/2025]
* Last Modified: [17/11/2025] by José Cruz
*
* Description:
* Maneja el feedback visual y sonoro del sistema de salud del enemigo.
* Utiliza el AudioManager para reproducir sonidos centralizados y
* genera efectos visuales al recibir daño, curarse o morir.
*
* Hours Worked: [1]
*
* Dependencies:
* - EnemyHealth.cs
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
* - Los sonidos ya no deben asignarse aquí; se gestionan desde AudioManager.
* - Contiene la misma estructura que PlayerHealthFeedback.cs. Considerar
*   la posibilidad de una clase base común si se repite más código.
*******************************************************/

using UnityEngine;

public class EnemyHealthFeedback : MonoBehaviour
{
    // ======================= VARIABLES =======================

    [Header("REFERENCIAS")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private AudioManager audioManager; 

    [Header("EFECTOS VISUALES")]
    [SerializeField] private GameObject damageEffect; 
    [SerializeField] private GameObject healEffect; 
    [SerializeField] private GameObject deathEffect;

    [Header("CONFIGURACIÓN DE EFECTOS")]
    [SerializeField] private float effectLifetime = 2f; // Tiempo antes de destruir efectos visuales


    // ======================= MÉTODOS PRINCIPALES =======================

    /// <summary>
    /// Feedback al recibir daño.
    /// </summary>
    public void OnDamageFeedback()
    {
        audioManager?.EnemyTakeDamage();
        //TriggerEffect(damageEffect);
    }

    /// <summary>
    /// Feedback al recibir curación.
    /// </summary>
    public void OnHealFeedback()
    {
        audioManager?.EnemyHeal();
        //TriggerEffect(healEffect);
    }

    /// <summary>
    /// Feedback al morir.
    /// </summary>
    public void OnDeathFeedback()
    {
        audioManager?.EnemyDeath();
        //TriggerEffect(deathEffect);
    }


    // ======================= FUNCIONES AUXILIARES =======================

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


    // ======================= EVENTOS UNITY =======================

    private void Awake()
    {
        if (enemyHealth == null)
            enemyHealth = GetComponent<EnemyHealth>();

        if (audioManager == null)
            audioManager = FindAnyObjectByType<AudioManager>();
    }
}
