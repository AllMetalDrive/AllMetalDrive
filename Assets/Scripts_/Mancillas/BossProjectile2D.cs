/**
* Project: All Metal Drive
* Script: BossProjectile2D.cs
* Author: Adaptado por GitHub Copilot
* Created: 30/11/2025
*
* Description:
* Controla el comportamiento de las bolas de fuego lanzadas por el jefe en 2D.
* Se mueve en línea recta y, al impactar con el suelo, genera una zona de daño.
*
* Dependencies:
* - Rigidbody2D (IsKinematic = true)
* - Collider2D (IsTrigger activo)
* - DamageZone (Prefab asignado)
*/

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BossProjectile2D : MonoBehaviour
{
    [Header("CONFIGURACIÓN DE MOVIMIENTO")]
    public float speed = 15f;

    [Header("CONFIGURACIÓN DE COMBATE")]
    public int directDamage = 15;
    public GameObject fireZonePrefab;

    private Vector2 _direction;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        // _rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el objeto tiene un script para administrar salud
        HealthBase health = other.GetComponent<HealthBase>();
        if (health == null)
            return;

        if (other.CompareTag("Player"))
        {
            // TODO: Implementar lógica de daño directo
            // other.GetComponent<PlayerHealth>().TakeDamage(directDamage);
            //Destroy(gameObject);
            //return;

            health.TakeDamage(directDamage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            SpawnFireZone();
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 dir)
    {
        _direction = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void SpawnFireZone()
    {
        if (fireZonePrefab != null)
        {
            Instantiate(fireZonePrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("BossProjectile2D: No se ha asignado un fireZonePrefab.");
        }
    }
}
