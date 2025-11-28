/*******************************************************
* Project: [Nombre del Proyecto]
* Script: PlayerAttackManager.cs
* Author: [Nombre del Desarrollador]
* Created: [DD/MM/AAAA]
* Last Modified: [DD/MM/AAAA] by [Nombre del último editor]
*
* Description:
* Maneja el ataque del jugador instanciando proyectiles.
*******************************************************/

using UnityEngine;

public class PlayerAttack : AttackBase // Hereda de AttackBase
{
    // ======================= VARIABLES =======================

    [Header("REFERENCIAS")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;


    // ======================= IMPLEMENTACIÓN DEL ATAQUE =======================

    protected override void PerformAttack() // Implementa el método abstracto de AttackBase
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("PlayerAttackManager: No se asignó el prefab del proyectil.");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogWarning("PlayerAttackManager: No se asignó el firePoint.");
            return;
        }

        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation); // Instancia el proyectil en la posición y rotación del firePoint.
        
    }


    // ======================= EVENTOS UNITY =======================

    private void Update()
    {
        if (Input.GetButton("Fire1"))
            Attack();
    }
}
