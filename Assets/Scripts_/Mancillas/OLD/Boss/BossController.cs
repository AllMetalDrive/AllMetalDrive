/**
* Project: All Metal Drive
* Script: BossController.cs
* Author: [Tu Nombre]
* Created: 16/11/2025
* Last Modified: 27/11/2025 by [Tu Nombre]
*
* Description:
* Controlador avanzado para el jefe "Dragón".
* Implementa una máquina de estados para vuelo, posicionamiento y 3 tipos de ataques especiales.
* Gestiona animaciones y restricciones de arena en 3D.
*
* Dependencies:
* - Rigidbody (3D) IsKinematic = true.
* - BoxCollider (3D) para la Arena.
* - Animator.
*
* Sections:
* - VARIABLES HEADER
* - EVENTOS UNITY
* - MÉTODOS PRINCIPALES (Máquina de Estados)
* - RUTINAS DE ATAQUE (Corrutinas)
* - FUNCIONES AUXILIARES
*
********/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class BossController : MonoBehaviour
{
    // =======================================================
    // =================== VARIABLES HEADER ==================
    // =======================================================

    [Header("ESTADÍSTICAS")]
    public float maxHealth = 100f;
    public float moveSpeed = 8f;
    // Tiempo entre ataques
    public float attackCooldown = 2f;

    [Header("VUELO IDLE")]
    // Altura a la que el dragón prefiere volar
    public float hoverHeight = 5f;
    // Suavizado de movimiento (Lerp)
    public float movementSmoothing = 2f;

    [Header("ATAQUE: BOLAS DE FUEGO")]
    public GameObject fireballPrefab;
    public Transform mouthFirePoint;
    // Tiempo entre cada bola de la ráfaga
    public float fireballBurstDelay = 0.3f;

    [Header("ATAQUE: EMBESTIDA (DASH)")]
    public float dashSpeed = 25f;
    // Tiempo de preparación antes de embestir
    public float dashPrepareTime = 1f;

    [Header("ATAQUE: CAÍDA (SLAM)")]
    public float slamSpeed = 30f;
    // Tiempo que se queda en el suelo tras caer
    public float slamRecoveryTime = 1.5f;

    [Header("REFERENCIAS")]
    public Transform playerTransform;
    public BoxCollider arenaBoundary;
    public Animator animator;

    // --- Variables Privadas ---
    private Rigidbody _rb;
    private bool _isActivated = false;
    private bool _isAttacking = false;
    private float _attackTimer;

    // Hashes de Animación para optimización
    private readonly int _animIsFlying = Animator.StringToHash("isFlying");
    private readonly int _animAttackFireball = Animator.StringToHash("attackFireball");
    private readonly int _animAttackDash = Animator.StringToHash("attackDash");
    private readonly int _animAttackSlam = Animator.StringToHash("attackSlam");

    // =======================================================
    // =================== EVENTOS UNITY =====================
    // =======================================================

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true; // Control total por script
        
        // Bloquear rotaciones indeseadas
        _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    private void Start()
    {
        // Inicia volando
        animator.SetBool(_animIsFlying, true);
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // Detección simple para activar al jefe
        if (!_isActivated)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance <= 15f) // Rango fijo o variable pública
            {
                _isActivated = true;
                _attackTimer = attackCooldown; // Dar un respiro inicial
            }
            return;
        }

        // Si está atacando, no hacemos lógica de Idle
        if (_isAttacking) return;

        HandleIdleMovement();
        HandleCombatLogic();
    }

    // =======================================================
    // ================= MÉTODOS PRINCIPALES =================
    // =======================================================

    /// <summary>
    /// Mantiene al jefe flotando y siguiendo suavemente al jugador en X.
    /// </summary>
    private void HandleIdleMovement()
    {
        // 1. Calcular posición objetivo (misma X que jugador, altura fija)
        // Usamos la Y del borde inferior de la arena + hoverHeight
        float targetY = arenaBoundary.bounds.min.y + hoverHeight;
        Vector3 targetPos = new Vector3(playerTransform.position.x, targetY, 0);

        // 2. Restringir objetivo dentro de la arena
        targetPos = GetClampedPosition(targetPos);

        // 3. Moverse suavemente hacia allí
        Vector3 smoothedPos = Vector3.Lerp(transform.position, targetPos, movementSmoothing * Time.deltaTime);
        _rb.MovePosition(smoothedPos);

        // 4. Mirar al jugador
        FacePlayer();
    }

    /// <summary>
    /// Decide cuándo y qué ataque lanzar.
    /// </summary>
    private void HandleCombatLogic()
    {
        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0)
        {
            ChooseAttack();
            _attackTimer = attackCooldown;
        }
    }

    private void ChooseAttack()
    {
        // Elegir aleatoriamente entre 3 ataques
        int rand = Random.Range(0, 3);

        switch (rand)
        {
            case 0:
                StartCoroutine(FireballAttackRoutine());
                break;
            case 1:
                StartCoroutine(HorizontalDashAttackRoutine());
                break;
            case 2:
                StartCoroutine(VerticalSlamAttackRoutine());
                break;
        }
    }

    // =======================================================
    // ================= RUTINAS DE ATAQUE ===================
    // =======================================================

    /// <summary>
    /// Ráfaga de 3 bolas de fuego desde el aire.
    /// </summary>
    private IEnumerator FireballAttackRoutine()
    {
        _isAttacking = true;
        
        // Trigger animación disparo
        animator.SetTrigger(_animAttackFireball);

        // Pequeña espera para sincronizar con la animación
        yield return new WaitForSeconds(0.5f);

        // Disparar 3 veces
        for (int i = 0; i < 3; i++)
        {
            if (fireballPrefab != null && mouthFirePoint != null)
            {
                GameObject fireball = Instantiate(fireballPrefab, mouthFirePoint.position, Quaternion.identity);
                // Calcular dirección hacia el jugador
                Vector3 direction = (playerTransform.position - mouthFirePoint.position).normalized;
                
                // Asignar dirección al script del proyectil
                BossProjectile projScript = fireball.GetComponent<BossProjectile>();
                if (projScript != null) projScript.SetDirection(direction);
            }
            yield return new WaitForSeconds(fireballBurstDelay);
        }

        // Esperar un momento antes de volver a moverse
        yield return new WaitForSeconds(1f);

        _isAttacking = false;
        animator.SetBool(_animIsFlying, true); // Asegurar vuelo
    }

    /// <summary>
    /// Se posiciona a un lado y embiste horizontalmente.
    /// </summary>
    private IEnumerator HorizontalDashAttackRoutine()
    {
        _isAttacking = true;

        // 1. Decidir lado (Izquierda o Derecha) basado en espacio disponible
        bool startFromLeft = playerTransform.position.x > arenaBoundary.bounds.center.x;
        float startX = startFromLeft ? arenaBoundary.bounds.min.x + 2f : arenaBoundary.bounds.max.x - 2f;
        float endX = startFromLeft ? arenaBoundary.bounds.max.x - 2f : arenaBoundary.bounds.min.x + 2f;
        
        // Altura del jugador
        Vector3 startPos = new Vector3(startX, playerTransform.position.y, 0);
        
        // 2. Moverse a posición de inicio (Rápido pero suave)
        yield return StartCoroutine(MoveToPosition(startPos, 1.5f));

        // 3. Preparar (Animación y espera)
        FacePoint(new Vector3(endX, transform.position.y, 0));
        animator.SetBool(_animIsFlying, true); // O una anim de "cargar"
        // Telegrafiar el ataque (puedes poner un sonido aquí)
        yield return new WaitForSeconds(dashPrepareTime);

        // 4. Embestida (Animación de ataque + Movimiento veloz)
        animator.SetTrigger(_animAttackDash);
        
        float time = 0;
        Vector3 initialDashPos = transform.position;
        Vector3 targetDashPos = new Vector3(endX, transform.position.y, 0);

        while (time < 1f)
        {
            time += Time.deltaTime * (dashSpeed / 10f); // Factor de velocidad
            _rb.MovePosition(Vector3.Lerp(initialDashPos, targetDashPos, time));
            yield return null;
        }

        // 5. Recuperación y volver a Idle
        yield return new WaitForSeconds(0.5f);
        _isAttacking = false;
    }

    /// <summary>
    /// Se posiciona arriba y cae verticalmente.
    /// </summary>
    private IEnumerator VerticalSlamAttackRoutine()
    {
        _isAttacking = true;

        // 1. Posicionarse encima del jugador (en el techo de la arena)
        Vector3 hoverPos = new Vector3(playerTransform.position.x, arenaBoundary.bounds.max.y - 1f, 0);
        hoverPos = GetClampedPosition(hoverPos);

        yield return StartCoroutine(MoveToPosition(hoverPos, 1.0f));

        // 2. Preparar caída
        // Detener animación de aleteo si deseas, o poner pose de caída
        yield return new WaitForSeconds(0.5f);

        // 3. Caída (Slam)
        animator.SetTrigger(_animAttackSlam);
        Vector3 groundPos = new Vector3(transform.position.x, arenaBoundary.bounds.min.y + 1f, 0);
        
        float time = 0;
        Vector3 startDrop = transform.position;

        while (Vector3.Distance(transform.position, groundPos) > 0.5f)
        {
            // Movimiento lineal rápido hacia abajo
            Vector3 newPos = Vector3.MoveTowards(transform.position, groundPos, slamSpeed * Time.deltaTime);
            _rb.MovePosition(newPos);
            yield return null;
        }

        // 4. Impacto y recuperación (quedarse en el suelo)
        // Aquí podrías instanciar ondas de choque o daño de área
        // animator.SetTrigger("Land"); // Si tuvieras animación de aterrizaje
        yield return new WaitForSeconds(slamRecoveryTime);

        // 5. Volver a volar
        animator.SetBool(_animIsFlying, true);
        _isAttacking = false;
    }

    // =======================================================
    // ================= FUNCIONES AUXILIARES ================
    // =======================================================

    /// <summary>
    /// Corrutina auxiliar para mover al jefe a un punto específico antes de atacar.
    /// </summary>
    private IEnumerator MoveToPosition(Vector3 target, float duration)
    {
        Vector3 startPos = transform.position;
        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime / duration;
            _rb.MovePosition(Vector3.Lerp(startPos, target, time));
            
            // Orientarse hacia donde se mueve
            if (target.x > transform.position.x) transform.localScale = new Vector3(1, 1, 1);
            else transform.localScale = new Vector3(-1, 1, 1);
            
            yield return null;
        }
    }

    private Vector3 GetClampedPosition(Vector3 pos)
    {
        if (arenaBoundary == null) return pos;
        Bounds b = arenaBoundary.bounds;
        return new Vector3(
            Mathf.Clamp(pos.x, b.min.x, b.max.x),
            Mathf.Clamp(pos.y, b.min.y, b.max.y),
            0
        );
    }

    private void FacePlayer()
    {
        if (playerTransform.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }
    
    private void FacePoint(Vector3 point)
    {
        if (point.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    /// <summary>
    /// Detecta colisión física con el jugador (para el Dash y Slam).
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (_isAttacking && collision.gameObject.CompareTag("Player"))
        {
            // Lógica de daño al tocar al jugador
            // collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(20);
            Debug.Log("¡Jefe golpeó al jugador con el cuerpo!");
        }
    }
}