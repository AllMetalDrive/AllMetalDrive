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

        [Header("CONFIGURACIÓN DE PROYECTIL")]
        [SerializeField] private float projectileDamage = 1f;
        [SerializeField] private float projectileSpeed = 15f;



    // ==================================================
    // ================= AIMING LOGIC ====================
    // ==================================================

    // El apuntado se maneja de forma independiente en AimAtMouse.cs



    // ==================================================
    // =============== MÉTODOS PRINCIPALES ==============
    // ==================================================

    /// <summary>
    /// Ejecuta el ataque creando un proyectil y asignándole
    /// el tipo de atacante correspondiente.
    /// </summary>
    protected override void PerformAttack()
    {
        if (firePoint == null)
            return;

        ProjectilePool pool = FindObjectOfType<ProjectilePool>();
        if (pool == null)
            return;

        Projectile_ proj = pool.GetPlayerProjectile(firePoint.position, firePoint.rotation);
        if (proj != null)
        {
            proj.isBossProjectile = false;
            proj.damage = (int)projectileDamage;
            proj.speed = projectileSpeed;
            proj.Initialize(shooterType);
            // Asigna siempre la dirección del disparo
            proj.SetDirection(firePoint.right);
        }

        // --- Play shooting sound depending on who fires ---
        if (audioManager != null)
        {
            if (shooterType == ShooterType.Player)
                audioManager.PlayerShoot();
            else if (shooterType == ShooterType.Enemy)
                audioManager.EnemyShoot();
        }
    }



    // ==================================================
    // =================== EVENTOS UNITY ================
    // ==================================================

    private void Update()
    {
        if (GameManagerUpdated.Instance.CurrentState != GameManagerUpdated.GameState.Gameplay)
            return; // No permitir ataque si no estamos en estado Gameplay

        // El apuntado se maneja de forma independiente en AimAtMouse.cs

        // --- Disparar ---
        if (Input.GetButton("Fire1"))
            Attack();   // Método heredado de AttackBase
    }
}
