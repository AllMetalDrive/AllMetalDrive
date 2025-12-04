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
* - Requiere configuración adecuada de etiquetas ("Player", "Enemy").
* - Requiere un componente Rigidbody2D para el movimiento.
* - La dirección del proyectil depende del eje transform.right.
* - Destruir el proyectil tras impacto puede habilitarse si el gameplay lo requiere.
*******************************************************/

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile_ : MonoBehaviour
{
    // ==================================================
    // ===================== VARIABLES ===================
    // ==================================================

    [Header("PROPIEDADES DEL PROYECTIL")]
    [SerializeField] public float speed = 10f;
    [SerializeField] public int damage = 1;
    [Header("EXTRA PARA JEFE")]
    [SerializeField] public GameObject fireZonePrefab;
    [SerializeField] public bool isBossProjectile = false;

    private Rigidbody2D rb;
    private ShooterType shooterType;
    private Vector2 _direction;



    // ==================================================
    // ================= MÉTODOS PRINCIPALES ============
    // ==================================================

    /// <summary>
    /// Recibe quién disparó el proyectil.
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
        // Si es proyectil normal, usa transform.right
        if (!isBossProjectile)
            rb.linearVelocity = transform.right * speed;
        // Si es proyectil de jefe, usa dirección asignada
        else
            rb.linearVelocity = _direction * speed;
        // Autodestrucción opcional
        //Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        if (isBossProjectile)
            rb.linearVelocity = _direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HealthBase health = other.GetComponent<HealthBase>();
        if (isBossProjectile)
        {
            // 1) Si impacta al jugador → daño directo
            if (other.CompareTag("Player"))
            {
                if (health != null)
                    health.TakeDamage(damage);
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
        else
        {
            // Si impacta un muro, pared u obstáculo (por tag o layer)
            bool hitWallTag = other.CompareTag("Wall");
            bool hitWallLayer = other.gameObject.layer == LayerMask.NameToLayer("Wall");
            if (hitWallTag || hitWallLayer)
            {
                ReturnToPool();
                Debug.Log("Proyectil impactó una pared u obstáculo. Uso de ReturnToPool().");
                return;
            }

            if (health == null)
                return;
            // Daño de Player → Enemy
            if (shooterType == ShooterType.Player && other.CompareTag("Enemy"))
            {
                health.TakeDamage(damage);
                ReturnToPool();
            }
            // Daño de Enemy → Player
            else if (shooterType == ShooterType.Enemy && other.CompareTag("Player"))
            {
                health.TakeDamage(damage);
                ReturnToPool();
            }
        }
    }
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
            Debug.LogWarning("Projectile_: No se ha asignado un fireZonePrefab.");
    }



    // ==================================================
    // ================= FUNCIONES AUXILIARES ===========
    // ==================================================

    private void ReturnToPool()
    {
        // Desactiva el proyectil
        gameObject.SetActive(false);

        // Solo los proyectiles del jugador usan pool
        if (!isBossProjectile)
        {
            ProjectilePool pool = FindObjectOfType<ProjectilePool>();
            if (pool != null)
                pool.ReturnPlayerProjectile(this);
            else
                Destroy(gameObject); // Fallback si no existe pool
        }
        else
        {
            Destroy(gameObject);
        }
    }
}



// ==================================================
// =================== ENUMERACIONES =================
// ==================================================

public enum ShooterType
{
    Player,
    Enemy
}
