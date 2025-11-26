/**
* Project: All Metal Drive 
* Script: Projectile.cs
* Author: Eduardo de Jes�s Mancillas Garc�a
* Created: 16/11/2025
*Last Modified: 16/11/2025 by Eduardo Mancillas
*
* Description:
* Script base para todos los proyectiles. Controla el movimiento,
* el tiempo de vida (rango) y el da�o.
*
*Hours Worked: 1
* Dependencies:
* - Rigidbody2D (requerido en el prefab)
* - Collider2D (con "Is Trigger" o normal)
*
* Sections:
* - VARIABLES
* - EVENTOS UNITY
*
********/

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    // =======================================================
    // =================== VARIABLES HEADER ==================
    // =======================================================

    [Header("ESTAD�STICAS DEL PROYECTIL")]
    // Cantidad de da�o que inflige este proyectil
    public float damage = 10f;

    // Tiempo de vida en segundos (define el rango)
    // Rango corto (Escopeta) = ~0.25f
    // Rango medio (Sencilla) = ~0.75f
    // Rango largo (Pesada)   = ~1.5f
    public float lifetime = 1.0f;

    // Velocidad a la que se mueve el proyectil
    public float projectileSpeed = 25f;
    
 
    [Header("REFERENCIAS")]
    // Efecto de part�cula al impactar (Opcional)
    [SerializeField] private GameObject hitEffect;

    // --- Variables Privadas ---
    private Rigidbody2D _rb;
    private Vector2 _direction;

    // =======================================================
    // =================== EVENTOS UNITY =====================
    // =======================================================

    /// <summary>
    /// Inicializa referencias y destruye el objeto despu�s de su "lifetime".
    /// </summary>
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    /// <summary>
    /// Mueve el proyectil usando f�sicas.
    /// </summary>
    private void FixedUpdate()
    {
        // Mover el proyectil usando su velocidad de Rigidbody
        _rb.linearVelocity = _direction * projectileSpeed;
    }

    /// <summary>
    /// Detecta colisiones (si no es Trigger).
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: L�gica de da�o
        // if (collision.gameObject.CompareTag("Enemy"))
        // {
        //     collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        // }

        HandleImpact();
    }

    /// <summary>
    // Detecta colisiones (si es Trigger).
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // TODO: L�gica de da�o
        // if (other.CompareTag("Enemy"))
        // {
        //     other.GetComponent<EnemyHealth>().TakeDamage(damage);
        // }

        // Ignorar colisiones con el jugador
        if (other.CompareTag("Player")) return;

        HandleImpact();
    }


    // =======================================================
    // ================= FUNCIONES AUXILIARES ================
    // =======================================================

    /// <summary>
    /// Llamado por el PlayerController para asignar la direcci�n inicial.
    /// </summary>
    public void SetDirection(Vector2 newDirection)
    {
        _direction = newDirection.normalized;
    }

    /// <summary>
    /// Gestiona lo que pasa al impactar (efectos, destrucci�n).
    /// </summary>
    private void HandleImpact()
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        // Desactivar el Rigidbody para que no se mueva mas al impactar
        _rb.linearVelocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Kinematic;
    
        // Desactivar el collider
        GetComponent<Collider2D>().enabled = false;

        // Destruir el objeto (quiz�s despu�s de una breve animaci�n del hitEffect)
        // Por ahora, lo destruimos de inmediato
        Destroy(gameObject);
    }
}