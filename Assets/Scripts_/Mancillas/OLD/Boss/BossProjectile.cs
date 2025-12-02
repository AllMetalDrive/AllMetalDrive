/**
* Project: All Metal Drive
* Script: BossProjectile.cs
* Author: Eduardo de Jesus Mancillas Garcia
* Created: 28/11/2025
* Last Modified: 28/11/2025 by Eduardo de Jesus Mancillas Garcia
*
* Description:
* Controla el comportamiento de las bolas de fuego lanzadas por el jefe.
* Se mueve en línea recta y, al impactar con el suelo, genera una zona de daño.
*
* Hours Worked: 0.5h
*
* Dependencies:
* - Rigidbody (3D)
* - Collider (3D) con IsTrigger activo
* - DamageZone (Prefab asignado)
*
* Sections:
* - VARIABLES HEADER
* - EVENTOS UNITY
* - MÉTODOS PRINCIPALES
* - FUNCIONES AUXILIARES
*
* Notes / Warnings:
* - Unity 6: Usa linearVelocity para el movimiento físico.
* - El objeto debe tener un Rigidbody con UseGravity = false.
********/

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BossProjectile : MonoBehaviour
{
    // =======================================================
    // =================== VARIABLES HEADER ==================
    // =======================================================

    [Header("CONFIGURACIÓN DE MOVIMIENTO")]
    // Velocidad de vuelo del proyectil
    public float speed = 15f; 

    [Header("CONFIGURACIÓN DE COMBATE")]
    // Daño directo si golpea al jugador
    public float directDamage = 15f; 
    // Prefab de la zona de fuego que se crea al impactar suelo
    public GameObject fireZonePrefab; 

    // --- Variables Privadas ---
    private Vector3 _direction;
    private Rigidbody _rb;

    // =======================================================
    // =================== EVENTOS UNITY =====================
    // =======================================================

    /// <summary>
    /// Inicializa referencias y configuraciones físicas.
    /// </summary>
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false; 
    }

    /// <summary>
    /// Actualiza el movimiento físico del proyectil.
    /// </summary>
    private void FixedUpdate()
    {
        // UNITY 6: Uso de linearVelocity
        _rb.linearVelocity = _direction * speed;
    }

    /// <summary>
    /// Gestiona las colisiones con el jugador o el entorno.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Si golpea al jugador directamente
        if (other.CompareTag("Player"))
        {
            // TODO: Implementar lógica de daño directo
            // other.GetComponent<PlayerHealth>().TakeDamage(directDamage);
            Destroy(gameObject);
            return;
        }

        // Si golpea el suelo (Verificar tag o layer)
        if (other.CompareTag("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Ground")) 
        {
            SpawnFireZone();
            Destroy(gameObject);
        }
    }

    // =======================================================
    // ================= FUNCIONES AUXILIARES ================
    // =======================================================

    /// <summary>
    /// Configura la dirección de vuelo del proyectil.
    /// </summary>
    /// <param name="dir">Vector normalizado de dirección.</param>
    public void SetDirection(Vector3 dir)
    {
        _direction = dir;
        
        // Rotar visualmente el proyectil hacia la dirección de movimiento
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    /// <summary>
    /// Instancia el prefab de la zona de fuego en la posición de impacto.
    /// </summary>
    private void SpawnFireZone()
    {
        if (fireZonePrefab != null)
        {
            // Instanciar la zona. Nota: Quaternion.identity asume que el prefab
            // ya viene plano o rotado correctamente para el suelo.
            Instantiate(fireZonePrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("BossProjectile: No se ha asignado un fireZonePrefab.");
        }
    }
}
