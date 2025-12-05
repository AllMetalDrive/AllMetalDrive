/*******************************************************
* Project: [All Metal Drive]
* Script: Projectile.cs
* Author: José Cruz
* Created: [15/11/2025]
* Last Modified: [16/11/2025] by José Cruz
*
* Description:
* Controla el comportamiento de un proyectil utilizando Rigidbody2D.
* El proyectil conoce quién lo disparó (Player o Enemy), se mueve en 
* línea recta, aplica daño al objetivo válido y se autodestruye.
*
* Hours Worked: [2]
*
* Dependencies:
* - Rigidbody2D (obligatorio)
* - HealthBase.cs (para aplicar daño)
* - ShooterType enum
*
* Sections:
* - VARIABLES
* - MÉTODOS PRINCIPALES
* - FUNCIONES AUXILIARES
* - EVENTOS UNITY
*
* Notes / Warnings:
* - Requiere Collider2D con "IsTrigger" activado.
* - Requere configuración adecuada de etiquetas ("Player", "Enemy").
* - Requiere un componente Rigidbody2D para el movimiento.
* - La dirección del proyectil depende del eje transform.right.
* - Destruir el proyectil tras impacto puede habilitarse si el gameplay lo requiere.
*******************************************************/

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Asegura que el GameObject tenga un Rigidbody2D.
public class Projectile_ : MonoBehaviour
{
    // ==================================================
    // ===================== VARIABLES ===================
    // ==================================================

    [Header("PROPIEDADES DEL PROYECTIL")]
    [SerializeField] private float speed = 10f;       // Velocidad lineal del proyectil
    [SerializeField] private int damage = 1;          // Daño infligido al impacto
    [SerializeField] private float lifetime = 3f;     // Tiempo antes de autodestruirse

    private Rigidbody2D rb;                           // Referencia al Rigidbody2D
    private ShooterType shooterType;                  // Quién disparó este proyectil (Player o Enemy)



    // ==================================================
    // ================= MÉTODOS PRINCIPALES ============
    // ==================================================

    /// <summary>
    /// Recibe la identidad del atacante que disparó el proyectil.
    /// </summary>
    public void Initialize(ShooterType shooter)
    {
        shooterType = shooter;
    }



    // ==================================================
    // =================== EVENTOS UNITY ================
    // ==================================================

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Aplicar velocidad inicial
        rb.linearVelocity = transform.right * speed;

        // Autodestrucción tras tiempo definido
        //Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el objeto tiene un script para administrar salud
        HealthBase health = other.GetComponent<HealthBase>();
        if (health == null)
            return;

        // Lógica de daño según quién disparó
        if (shooterType == ShooterType.Player && other.CompareTag("Enemy"))
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (shooterType == ShooterType.Enemy && other.CompareTag("Player"))
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}



// ==================================================
// =================== ENUMERACIONES =================
// ==================================================

public enum ShooterType // Identifica quién disparó el proyectil para aplicar daño correctamente.
{
    Player,
    Enemy
}
