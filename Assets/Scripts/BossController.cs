/**
* Project: All Metal Drive
* Script: BossController.cs
* Author: [Tu Nombre]
* Created: 16/11/2025
* Last Modified: 16/11/2025 by [Tu Nombre]
*
* Description:
* Controlador principal para el jefe "Dragón Volador".
* Gestiona la detección del jugador, el movimiento básico de persecución
* y asegura que el jefe nunca salga del área designada (Arena).
* Prepara la estructura para futuros ataques de ráfaga.
*
* Hours Worked: 1h
*
* Dependencies:
* - Rigidbody2D (Kinematic recomendado para vuelo)
* - BoxCollider2D (Trigger) en un objeto externo que defina la "Arena".
*
* Sections:
* - VARIABLES HEADER
* - EVENTOS UNITY
* - MÉTODOS PRINCIPALES
* - FUNCIONES AUXILIARES
*
* Notes / Warnings:
* - El Rigidbody2D del jefe debe ser 'Kinematic' para controlar el vuelo
* sin que la gravedad lo afecte constantemente.
********/

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossController : MonoBehaviour // [cite: 13] PascalCase para clases
{
    // =======================================================
    // =================== VARIABLES HEADER ==================
    // =======================================================

    [Header("ESTADÍSTICAS")] // [cite: 60] Uso de Header en mayúsculas
    // Vida máxima del jefe (baja, estilo Glass Cannon)
    public float maxHealth = 100f; // [cite: 21] camelCase para públicas
    // Velocidad de movimiento en el aire
    public float moveSpeed = 12f;

    [Header("DETECCIÓN Y ARENA")]
    // Distancia a la que el jefe detecta al jugador y empieza la pelea
    public float detectionRange = 15f;
    // Referencia al Collider que define los límites de la zona de combate
    public BoxCollider2D arenaBoundary;

    [Header("REFERENCIAS")]
    // Transform del jugador
    public Transform playerTransform;

    [Header("DEBUG")]
    // Visualizar el radio de detección y límites en el editor
    [SerializeField] private bool _showGizmos = true; // [cite: 25] Privada con guión bajo

    // --- Variables Privadas (Estado) ---
    private Rigidbody2D _rb;
    private Vector2 _targetPosition;
    private bool _isActivated = false;

    // Enum para controlar la Máquina de Estados del Jefe
    private enum BossState { Idle, Chasing, Attacking }
    private BossState _currentState = BossState.Idle;

    // =======================================================
    // =================== EVENTOS UNITY =====================
    // =======================================================

    /// <summary>
    /// Inicializa referencias.
    /// </summary>
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Gestiona la lógica frame a frame dependiendo del estado.
    /// </summary>
    private void Update()
    {
        if (playerTransform == null) return;

        // Máquina de Estados
        switch (_currentState)
        {
            case BossState.Idle:
                HandleIdleState();
                break;
            case BossState.Chasing:
                HandleChasingState();
                break;
            case BossState.Attacking:
                // Lógica de ataque (se implementará en futuros mensajes)
                break;
        }
    }

    /// <summary>
    /// Aplica el movimiento y las restricciones físicas.
    /// </summary>
    private void FixedUpdate()
    {
        if (_isActivated && _currentState == BossState.Chasing)
        {
            MoveTowardsTarget();
        }

        // Siempre restringir posición, incluso si está atacando
        ClampPositionToArena();
    }

    /// <summary>
    /// Dibuja ayudas visuales en el editor.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!_showGizmos) return;

        // Dibujar rango de detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Dibujar borde de la arena si está asignada
        if (arenaBoundary != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(arenaBoundary.bounds.center, arenaBoundary.bounds.size);
        }
    }

    // =======================================================
    // ================= MÉTODOS PRINCIPALES =================
    // =======================================================

    /// <summary>
    /// Comportamiento cuando el jefe está esperando al jugador.
    /// </summary>
    private void HandleIdleState() // [cite: 17] PascalCase para métodos
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange)
        {
            ActivateBoss();
        }
    }

    /// <summary>
    /// Comportamiento de persecución/vuelo hacia el jugador.
    /// </summary>
    private void HandleChasingState()
    {
        // Por ahora, perseguimos directamente al jugador
        // En el futuro, aquí decidiremos si atacar o seguir moviéndonos
        _targetPosition = playerTransform.position;

        // Orientar al jefe (Flip)
        if (_targetPosition.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1); // Mirar derecha
        else
            transform.localScale = new Vector3(-1, 1, 1); // Mirar izquierda
    }

    // =======================================================
    // ================ FUNCIONES AUXILIARES =================
    // =======================================================

    /// <summary>
    /// Despierta al jefe y cambia al estado de combate.
    /// </summary>
    private void ActivateBoss()
    {
        _isActivated = true;
        _currentState = BossState.Chasing;
        Debug.Log("¡El Jefe ha despertado!");
    }

    /// <summary>
    /// Mueve al jefe hacia el objetivo usando física.
    /// </summary>
    private void MoveTowardsTarget()
    {
        // Calcular dirección
        Vector2 direction = (_targetPosition - (Vector2)transform.position).normalized;

        // Mover usando MovePosition para colisiones más seguras
        Vector2 newPos = _rb.position + direction * moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(newPos);
    }

    /// <summary>
    /// Mantiene al jefe estrictamente dentro de los límites del BoxCollider2D asignado.
    /// </summary>
    private void ClampPositionToArena()
    {
        if (arenaBoundary == null) return;

        Bounds bounds = arenaBoundary.bounds;

        // Obtener posición actual
        Vector3 clampedPos = transform.position;

        // Restringir X e Y usando Mathf.Clamp
        // Se añade un pequeño margen (e.g., el tamaño del jefe) si fuera necesario,
        // pero aquí usamos los límites exactos del collider.
        clampedPos.x = Mathf.Clamp(clampedPos.x, bounds.min.x, bounds.max.x);
        clampedPos.y = Mathf.Clamp(clampedPos.y, bounds.min.y, bounds.max.y);

        // Aplicar posición restringida
        transform.position = clampedPos;
    }
}