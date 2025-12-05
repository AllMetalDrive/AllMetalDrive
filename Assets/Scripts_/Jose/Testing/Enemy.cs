/*******************************************************
* Project: [Nombre del Proyecto]
* Script: Enemy.cs
* Author: [Nombre del Desarrollador]
* Created: [DD/MM/AAAA]
* Last Modified: [DD/MM/AAAA] by [Nombre del último editor]
*
* Description:
* Script básico para un enemigo. Detecta al jugador mediante
* un collider y aplica daño usando TakeDamage() del PlayerHealth.
*
* Hours Worked: [Total de horas aproximadas dedicadas]
*
* Dependencies:
* - PlayerHealth.cs
*
* Sections:
* - VARIABLES
* - MÉTODOS PRINCIPALES
* - FUNCIONES AUXILIARES
* - EVENTOS UNITY
*
* Notes / Warnings:
* - Asegurarse que el jugador tenga la etiqueta definida en PLAYER_TAG.
* - El enemigo debe tener un collider con "Is Trigger" activado.
*******************************************************/


using UnityEngine;

public class Enemy : MonoBehaviour
{
    // ======================= VARIABLES =======================

    [Header("TAG DEL OBJETIVO")] // Solo impactará con objetos que tengan este tag
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private int damage = 1;      // Daño que inflige al objetivo



    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto NO tiene el tag objetivo → ignorar
        if (!other.CompareTag(targetTag))
            return;

        // Intentar aplicar daño a cualquier objeto derivado de HealthBase
        HealthBase health = other.GetComponent<HealthBase>();

        if (health != null)
            health.TakeDamage(damage);

        // Destruye el proyectil después del impacto exitoso
        //Destroy(gameObject);
    }
}

