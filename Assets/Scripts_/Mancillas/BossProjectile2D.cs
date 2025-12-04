/**
* Project: All Metal Drive
* Script: BossProjectile2D.cs
* Author: Adaptado por GitHub Copilot / Corregido por ChatGPT
* Created: 30/11/2025
*
* Description:
* Controla el comportamiento de las bolas de fuego lanzadas por el jefe en 2D.
* Se mueve en línea recta y, al impactar con el suelo, genera una zona de daño.
*
* Dependencies:
* - Rigidbody2D (IsKinematic = true recomendado)
* - Collider2D (IsTrigger activo)
* - DamageZone (Prefab asignado)
*/

/*
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BossProjectile2D : Projectile_
{
    // ==================================================
    // ===================== VARIABLES ===================
    // ==================================================

    [Header("CONFIGURACIÓN DE MOVIMIENTO")]
    public float speed = 15f;

    [Header("CONFIGURACIÓN DE COMBATE")]
    public float directDamage = 15f;
    public GameObject fireZonePrefab;

    private Vector2 _direction;
    private Rigidbody2D _rb;



    // ==================================================
    // =================== EVENTOS UNITY ================
    // ==================================================

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        //_rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1) Si impacta al jugador → daño directo
        if (other.CompareTag("Player"))
        {
            HealthBase health = other.GetComponent<HealthBase>();
            if (health != null)
            {
                health.TakeDamage(directDamage);
            }

            ReturnToPool();
            return;
        }

        // 2) Si impacta el suelo → crear zona de fuego
        bool hitGroundTag = other.CompareTag("Ground");
        bool hitGroundLayer = other.gameObject.layer == LayerMask.NameToLayer("Ground");

        if (hitGroundTag || hitGroundLayer)
        {
            SpawnFireZone();
            ReturnToPool();
            return;
        }
    }



    // ==================================================
    // ================= FUNCIONES AUXILIARES ===========
    // ==================================================

    public void SetDirection(Vector2 dir)
    {
        _direction = dir.normalized;

        // Rotación del sprite según dirección
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void SpawnFireZone()
    {
        if (fireZonePrefab != null)
            Instantiate(fireZonePrefab, transform.position, Quaternion.identity);
        else
            Debug.LogWarning("BossProjectile2D: No se ha asignado un fireZonePrefab.");
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);

        // Intentar devolver al pool del jefe
        ProjectilePool pool = FindObjectOfType<ProjectilePool>();
        if (pool != null)
        {
            pool.ReturnProjectile(this);  // ESTE proyectil (BossProjectile2D)
        }
        else
        {
            Destroy(gameObject); // Fallback si no existe pool
        }
    }
}
*/