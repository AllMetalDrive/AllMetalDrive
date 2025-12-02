/*******************************************************
* Project: [All Metal Drive]
* Script: AttackBase.cs
* Author: Jose Cruz
* Created: 15/11/2025
* Last Modified: 16/11/2025 by Jose Cruz
*
* Description:
* Clase base abstracta que maneja la lógica de enfriamiento
* (cooldown) de ataques. NO ejecuta ataques por sí misma.
* Las clases hijas implementan el comportamiento concreto 
* (proyectiles, melee, raycast, etc.).
*
* Hours Worked: [2]
*
* Dependencies:
* - ProjectileAttack
* - MeleeAttack
*
* Sections:
* - VARIABLES
* - MÉTODOS PRINCIPALES
* - FUNCIONES AUXILIARES
*
* Notes / Warnings:
* - Attack() solo se llama desde controladores externos 
*   (PlayerController, EnemyAI, etc.).
*******************************************************/

using UnityEngine;

public abstract class AttackBase : MonoBehaviour
{
    // ==================================================
    // ===================== VARIABLES ===================
    // ==================================================

    [Header("COOLDOWN DE ATAQUE")]
    [SerializeField] protected float attackRate = 0.25f;    // Tiempo mínimo entre ataques
    protected float nextAttackTime = 0f;                    // Momento en que puede volver a atacar

    [Header("QUIÉN DISPARA ESTE ATAQUE?")]
    [SerializeField] protected ShooterType shooterType = ShooterType.Player; // Define si es jugador o enemigo

    public ShooterType ShooterType => shooterType;          // Acceso público de solo lectura



    // ==================================================
    // =============== MÉTODOS PRINCIPALES ==============
    // ==================================================

    /// <summary>
    /// Método llamado por controladores externos 
    /// (Player, Enemy, IA, etc.). Verifica cooldown
    /// y ejecuta PerformAttack() si es válido.
    /// </summary>
    public void Attack()
    {
        if (!CanAttack())
            return;

        PerformAttack();
        nextAttackTime = Time.time + attackRate;
    }

    /// <summary>
    /// Método abstracto implementado por las clases hijas.
    /// Define la lógica del ataque (proyectiles, melee, etc.).
    /// </summary>
    protected abstract void PerformAttack();



    // ==================================================
    // ============= FUNCIONES AUXILIARES ===============
    // ==================================================

    /// <summary>
    /// Verifica si el cooldown ha terminado.
    /// </summary>
    protected virtual bool CanAttack()
    {
        return Time.time >= nextAttackTime;
    }
}
