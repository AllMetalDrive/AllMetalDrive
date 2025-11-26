/**
* Project: All Metal Drive 
* Script: PlayerController.cs
* Author: Eduardo de Jes�s Mancillas Garc�a
* Created: 16/11/2025
* Last Modified: 16/11/2025 by Eduardo Mancillas
*
* Description:
* Controla el movimiento 2.5D (movimiento 2D) y el combate del jugador.
* Gestiona el movimiento, salto, dash y un sistema de 3 armas intercambiables.
* Implementa un estado de "disparo" donde el movimiento se bloquea y las teclas
* de movimiento se usan para apuntar en 8 direcciones.
*
* Hours Worked: 2
*
* Dependencies:
* - Rigidbody2D (requerido en el mismo GameObject)
* - Projectile.cs (script requerido en los prefabs de proyectil)
* - Un "groundCheck" (GameObject) hijo para detectar el suelo.
* - Un "firePoint" (GameObject) hijo para instanciar proyectiles.
*
* Sections:
* - VARIABLES (agrupadas con [Header()])
* - EVENTOS UNITY (Awake, Update, FixedUpdate)
* - M�TODOS PRINCIPALES (Manejo de Inputs y Estados) 
* - FUNCIONES AUXILIARES (Movimiento, Combate, Verificaciones) 
*
* Notes / Warnings:
* - El jugador debe tener una capa ("Layer") asignada que NO est�
* incluida en la "groundLayer" para que el GroundCheck funcione.
* - Los proyectiles deben ser prefabs con el script Projectile.cs.
* - El movimiento horizontal se bloquea al disparar (mec�nica solicitada).
********/

using UnityEngine;
using System.Collections.Generic; // Necesario para la lista de armas

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // =======================================================
    // =================== VARIABLES HEADER ==================
    // =======================================================

    [Header("MOVIMIENTO")]
    // Velocidad de movimiento horizontal del jugador
    public float moveSpeed = 8f;
    // Fuerza aplicada al saltar
    public float jumpForce = 15f;
    // Modificador de gravedad al caer (para un salto m�s r�pido)
    public float fallMultiplier = 2.5f;
    // Modificador de gravedad al soltar el salto (salto corto)
    public float lowJumpMultiplier = 2f;

    [Header("DASH")]
    // Velocidad (fuerza) del impulso del dash
    public float dashSpeed = 20f;
    // Duraci�n del impulso del dash en segundos
    public float dashDuration = 0.2f;
    // Tiempo de espera (cooldown) entre dashes en segundos 
    public float dashCooldown = 1f;

    [Header("COMBATE")]
    // Lista de las 3 armas (ScriptableObjects o Prefabs)
    // NOTA: Para este ejemplo, usaremos prefabs de proyectil directos.
    // Prefab del proyectil del Arma 1 (Sencilla)
    public GameObject weapon1Projectile;
    // Cadencia del Arma 1 (disparos por segundo)
    public float weapon1FireRate = 2f;

    // Prefab del proyectil del Arma 2 (Escopeta)
    public GameObject weapon2Projectile;
    // Cadencia del Arma 2
    public float weapon2FireRate = 1f;
    // Cantidad de proyectiles (perdigones) de la escopeta
    public int shotgunPelletCount = 5;
    // �ngulo de dispersi�n (en grados) de la escopeta
    public float shotgunSpreadAngle = 15f;

    // Prefab del proyectil del Arma 3 (Pesada)
    public GameObject weapon3Projectile;
    // Cadencia del Arma 3 (lenta)
    public float weapon3FireRate = 0.5f;

    [Header("REFERENCIAS")]
    // Punto desde donde se originan los disparos
    [SerializeField] private Transform firePoint;
    // Objeto hijo que detecta si el jugador est� en el suelo
    [SerializeField] private Transform groundCheck;

    [Header("CONFIGURACI�N DE F�SICA")]
    // Radio del c�rculo para detectar el suelo
    [SerializeField] private float groundCheckRadius = 0.2f;
    // Qu� capas ("Layers") se consideran "suelo"
    [SerializeField] private LayerMask groundLayer;

    // --- Variables Privadas (Estado) ---
    private Rigidbody2D _rb;
    private bool _isGrounded;
    private bool _isDashing;
    private bool _canDash = true;
    private float _dashTimer;
    private float _dashCooldownTimer;

    private bool _isShooting;
    private bool _facingRight = true;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector2 _aimDirection;

    private int _currentWeaponIndex = 0; // 0=W1, 1=W2, 2=W3
    private float _shootTimer;
    private GameObject[] _weaponProjectiles;
    private float[] _weaponFireRates;

    // =======================================================
    // =================== EVENTOS UNITY =====================
    // =======================================================

    /// <summary>
    /// Se llama una vez al inicio, antes del Start.
    /// Ideal para inicializar referencias.
    /// </summary>
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        // Inicializar arrays de armas para f�cil acceso
        _weaponProjectiles = new GameObject[] { weapon1Projectile, weapon2Projectile, weapon3Projectile };
        _weaponFireRates = new float[] { weapon1FireRate, weapon2FireRate, weapon3FireRate };

        _aimDirection = Vector2.right; // Apuntar a la derecha por defecto
    }

    /// <summary>
    /// Se llama en cada frame.
    /// Usado para manejar inputs y l�gica de estado.
    /// </summary>
    private void Update()
    {
        HandleInput();
        HandleTimers();
        HandleWeaponSwitch();
        HandleAiming();
    }

    /// <summary>
    /// Se llama en intervalos fijos de tiempo.
    /// Usado para la l�gica de f�sicas (movimiento, salto).
    /// </summary>
    private void FixedUpdate()
    {
        // Verificaciones de estado
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (_isDashing) return; // Si est� en dash, no hacer nada m�s

        HandleMovement();
        ApplyBetterGravity();
    }

    // =======================================================
    // ================= M�TODOS PRINCIPALES =================
    // =======================================================

    /// <summary>
    /// Centraliza la lectura de todos los inputs del jugador.
    /// </summary>
    private void HandleInput()
    {
        // Inputs de movimiento (usados para mover O para apuntar)
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        // Input de Disparo
        _isShooting = Input.GetButton("Fire1"); // "Fire1" es Click Izq o Ctrl Izq

        // Input de Salto
        if (Input.GetButtonDown("Jump") && (_isGrounded || _isShooting)) // Puede saltar si est� en el suelo O si est� disparando
        {
            HandleJump();
        }

        // Input de Dash

        if (Input.GetKeyDown(KeyCode.LeftShift)  && _canDash && !_isShooting) // "Dash" (mapear en Edit > Project Settings > Input Manager)
        {
            StartDash();
        }
    }
    

    /// <summary>
    /// Gestiona los contadores de tiempo para cooldowns (Dash, Disparo).
    /// </summary>
    private void HandleTimers()
    {
        // Cooldown de Disparo
        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
        }

        // Cooldown de Dash
        if (!_canDash)
        {
            _dashCooldownTimer -= Time.deltaTime;
            if (_dashCooldownTimer <= 0)
            {
                _canDash = true;
            }
        }

        // Duraci�n del Dash
        if (_isDashing)
        {
            _dashTimer -= Time.deltaTime;
            if (_dashTimer <= 0)
            {
                StopDash();
            }
        }
    }

    /// <summary>
    /// Actualiza la direcci�n de apuntado (_aimDirection) basado en el input.
    /// Si est� disparando, usa 8 direcciones. Si no, usa la direcci�n horizontal.
    /// </summary>
    private void HandleAiming()
    {
        if (_isShooting)
        {
            // Si se est� disparando, los inputs (W,A,S,D) definen la mira
            float x = _horizontalInput;
            float y = _verticalInput;

            if (x == 0 && y == 0)
            {
                // Si no hay input, mantener la direcci�n horizontal actual
                _aimDirection = _facingRight ? Vector2.right : Vector2.left;
            }
            else
            {
                // Normalizar para obtener una direcci�n de 8 ejes
                _aimDirection = new Vector2(x, y).normalized;
            }

            // Disparar si el timer lo permite
            if (_shootTimer <= 0)
            {
                Shoot();
            }
        }
        else
        {
            // Si no est� disparando, la mira es solo horizontal
            if (_horizontalInput != 0)
            {
                _aimDirection = _horizontalInput > 0 ? Vector2.right : Vector2.left;
            }
        }
    }

    // =======================================================
    // ================= FUNCIONES AUXILIARES ================
    // =======================================================

    /// <summary>
    /// Aplica el movimiento horizontal o lo detiene si est� disparando.
    /// </summary>
    private void HandleMovement()
    {
        if (_isShooting)
        {
            // Bloquear movimiento horizontal mientras se dispara
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        }
        else
        {
            // Movimiento normal
            _rb.linearVelocity = new Vector2(_horizontalInput * moveSpeed, _rb.linearVelocity.y);

            // Flip (Girar el personaje)
            if (_horizontalInput > 0 && !_facingRight)
            {
                Flip();
            }
            else if (_horizontalInput < 0 && _facingRight)
            {
                Flip();
            }
        }
    }

    /// <summary>
    /// Aplica la fuerza de salto.
    /// </summary>
    private void HandleJump()
    {
        // Reseteamos la velocidad vertical para un salto consistente
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0f);
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Aplica una gravedad modificada para un mejor "game feel" del salto.
    /// </summary>
    private void ApplyBetterGravity()
    {
        if (_rb.linearVelocity.y < 0)
        {
            _rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (_rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            _rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    /// <summary>
    /// Inicia el estado de Dash.
    /// </summary>
    private void StartDash()
    {
        _isDashing = true;
        _canDash = false;
        _dashTimer = dashDuration;
        _dashCooldownTimer = dashCooldown;

        _rb.gravityScale = 0; // Ignorar gravedad durante el dash
        _rb.linearVelocity = new Vector2(_facingRight ? dashSpeed : -dashSpeed, 0);
    }

    /// <summary>
    /// Termina el estado de Dash.
    /// </summary>
    private void StopDash()
    {
        _isDashing = false;
        _rb.gravityScale = 1; // Restaurar gravedad (o tu valor original)
        _rb.linearVelocity = Vector2.zero; // Detenerse bruscamente
    }

    /// <summary>
    /// Gira el "transform" del jugador para que mire en la direcci�n opuesta.
    /// </summary>
    private void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    /// <summary>
    /// Revisa los inputs (1, 2, 3) para cambiar el arma activa.
    /// </summary>
    private void HandleWeaponSwitch()
    {
        if (_isShooting) return; // No se puede cambiar de arma mientras se dispara

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentWeaponIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentWeaponIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _currentWeaponIndex = 2;
        }
    }

    /// <summary>
    /// Funci�n central que decide qu� arma disparar.
    /// </summary>
    private void Shoot()
    {
        _shootTimer = 1f / _weaponFireRates[_currentWeaponIndex];

        GameObject prefabToSpawn = _weaponProjectiles[_currentWeaponIndex];
        if (prefabToSpawn == null)
        {
            Debug.LogWarning($"No hay prefab asignado para el Arma {_currentWeaponIndex + 1}");
            return;
        }

        switch (_currentWeaponIndex)
        {
            case 0: // Arma 1 (Sencilla)
                SpawnProjectile(prefabToSpawn, _aimDirection);
                break;
            case 1: // Arma 2 (Escopeta)
                for (int i = 0; i < shotgunPelletCount; i++)
                {
                    // Calcular dispersi�n
                    float spread = Random.Range(-shotgunSpreadAngle / 2, shotgunSpreadAngle / 2);
                    Quaternion spreadRotation = Quaternion.AngleAxis(spread, Vector3.forward);
                    Vector2 spreadDirection = spreadRotation * _aimDirection;

                    SpawnProjectile(prefabToSpawn, spreadDirection);
                }
                break;
            case 2: // Arma 3 (Pesada)
                SpawnProjectile(prefabToSpawn, _aimDirection);
                break;
        }
    }

    /// <summary>
    /// Instancia un proyectil en el firePoint con la direcci�n dada.
    /// </summary>
    /// <param name="prefab">El prefab del proyectil a instanciar.</param>
    /// <param name="direction">La direcci�n normalizada del disparo.</param>
 
    private void SpawnProjectile(GameObject prefab, Vector2 direction)
    {
        // Calcular la rotaci�n del proyectil basado en la direcci�n de la mira
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject projectile = Instantiate(prefab, firePoint.position, rotation);

        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.SetDirection(direction);
        }
    }
}