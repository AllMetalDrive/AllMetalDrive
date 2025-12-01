/*******************************************************
* Project: [All Metal Drive]
* Script: ProjectileAttack.cs
* Author: José Cruz
* Created: [16/11/2025]
* Last Modified: [16/11/2025] by José Cruz
*
* Description:
* Maneja un ataque basado en proyectiles, instanciándolos
* desde un punto de disparo según el cooldown definido 
* en AttackBase. La clase Projectile se encarga del
* movimiento, daño y facción del proyectil.
*
* Hours Worked: [1]
*
* Dependencies:
* - AttackBase.cs
* - Projectile.cs
* - ShooterType enum
*
* Sections:
* - VARIABLES
* - MÉTODOS PRINCIPALES
* - EVENTOS UNITY
*
* Notes / Warnings:
* - projectilePrefab debe contener un componente (script) Projectile.
* - firePoint determina dirección del disparo (transform.right).
*******************************************************/

using UnityEngine;

public class ProjectileAttack : AttackBase
{
    // ==================================================
    // ===================== VARIABLES ===================
    // ==================================================

    [Header("REFERENCIAS")]
    [SerializeField] private GameObject projectilePrefab;  // Prefab del proyectil a instanciar
    [SerializeField] private Transform firePoint;          // Punto desde donde se generan los disparos
    [SerializeField] private AudioManager audioManager;    // Referencia al AudioManager para reproducir sonidos



    // ==================================================
    // =============== MÉTODOS PRINCIPALES ==============
    // ==================================================

    /// <summary>
    /// Ejecuta el ataque creando un proyectil y asignándole
    /// el tipo de atacante correspondiente.
    /// </summary>
    protected override void PerformAttack()
    {
        if (projectilePrefab == null || firePoint == null)
            return;

        GameObject projObj = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        Projectile_ proj = projObj.GetComponent<Projectile_>();
        if (proj != null)
        {
            // Asignar facción del atacante desde AttackBase
            proj.Initialize(shooterType);
        }

        // --- Play shooting sound depending on who fires ---
        if (audioManager != null)
        {
            if (shooterType == ShooterType.Player)
            {
                audioManager.PlayerShoot();
            }
            else if (shooterType == ShooterType.Enemy)
            {
                audioManager.EnemyShoot();
            }
        }
    }



    // ==================================================
    // =================== EVENTOS UNITY ================
    // ==================================================

    private void Update()
    {
        // Input simple para disparos del jugador
        if (Input.GetButton("Fire1"))
            Attack();   // Método heredado de AttackBase
    }
}
