/*******************************************************
* Project: [Nombre del Proyecto]
* Script: MeleeAttack.cs
* Author: José Cruz
* Created: [16/11/2025]
* Last Modified: [16/11/2025] by José Cruz
*
* Description:
* Implementación de ataque cuerpo a cuerpo. Detecta un objetivo
* dentro de un rango definido y aplica daño mediante HealthBase.
*
* Hours Worked: [1]
*
* Dependencies:
* - AttackBase (clase padre)
* - HealthBase (para aplicar daño)
*
* Sections:
* - VARIABLES
* - MÉTODOS PRINCIPALES
* - FUNCIONES AUXILIARES
* - EVENTOS UNITY
*
* Notes / Warnings:
* - Requiere un LayerMask correctamente configurado para detectar objetivos.
* - El punto de origen del ataque es la posición del objeto actual.
*******************************************************/

using UnityEngine;

public class MeleeAttack : AttackBase
{
    // ======================= VARIABLES =======================

    [Header("ATAQUE MELEE")]
    [SerializeField] private int damage = 10;              // Daño del ataque
    [SerializeField] private float range = 1f;            // Radio del ataque
    [SerializeField] private LayerMask targetLayers;       // Capas que el ataque puede impactar


    // ======================= MÉTODOS PRINCIPALES =======================

    /// <summary>
    /// Implementación concreta del ataque melee. Busca un objetivo dentro
    /// del rango y aplica daño usando HealthBase.
    /// </summary>
    protected override void PerformAttack()
    {
        // Detecta un objetivo dentro del círculo de ataque
        Collider2D hit = Physics2D.OverlapCircle(transform.position, range, targetLayers);

        if (hit != null)
        {   
            Debug.Log($"MeleeAttack impactó a {hit.gameObject.name}");
            // Intenta obtener HealthBase del objetivo
            HealthBase health = hit.GetComponent<HealthBase>();

            // Si el objetivo tiene salud → aplicar daño
            if (health != null)
                health.TakeDamage(damage);
        }
    }


    // ======================= EVENTOS UNITY =======================

 /*   private void OnTriggerEnter2D(Collider2D other)
    {
        if (shooterType == ShooterType.Enemy && other.CompareTag("Player"))
        {
            Attack();
            
        }
    } */
    
    // TODO: Método de activación del ataque según la lógica del juego
    /* private void Update() { // Ejemplo de activación del ataque con la barra espaciadora

        if (Input.GetKeyDown(KeyCode.Space) && shooterType == ShooterType.Enemy)
        {
            Attack();
        }
    } */

    private void OnDrawGizmosSelected() // Dibuja el rango de ataque en la vista del editor
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
