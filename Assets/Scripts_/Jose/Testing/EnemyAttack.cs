/*******************************************************
* Script: EnemyAttack.cs
* Maneja el ataque del enemigo usando AttackBase

* Project: [Nombre del Proyecto]
* Script: EnemyAttack.cs
* Author: [Nombre del Desarrollador]
* Created: [DD/MM/AAAA]
* Last Modified: [DD/MM/AAAA] by [Nombre del último editor]
*
* Description:
* Maneja el ataque del enemigo usando AttackBase.
*******************************************************/

using UnityEngine;

public class EnemyAttack : AttackBase
{
    [Header("TAG DEL OBJETIVO")]
    [SerializeField] private string targetTag = "Player";

    
    protected override void PerformAttack()
    {
        // Este enemigo no dispara proyectiles en este ejemplo.
        // Su ataque ocurre solo al colisionar (hace contacto con el jugador).
        // Este método se deja intencionalmente vacío o se puede extender más adelante.
    }


    // ======================= EVENTOS UNITY =======================

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag))
            return;

        if (!CanAttack())
            return;

        HealthBase health = other.GetComponent<HealthBase>();

        if (health != null)
        {
            //health.TakeDamage(damage);
            nextAttackTime = Time.time + attackRate;
        }
    }
}
