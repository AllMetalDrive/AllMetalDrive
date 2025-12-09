/**
* Project: All Metal Drive 
* Script: PlayerController2D.cs
* Author: Eduardo de Jesús Mancillas García (Modified by Assistant)
* Created: 11/16/2025
* Last Modified: [FECHA_ACTUAL]
*
* Description:
* Controls the player's 2.5D movement, combat, and UI feedback.
* Now includes dash invincibility toggle and UI cooldown indicator.
********/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para controlar la Imagen del UI

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpForce = 12f;

    [Header("Better Jump Settings")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Dash Settings")]
    public float dashForce = 18f;
    public float dashCooldown = 1.2f;
    public float dashTime = 0.15f; // ¡Ahora es pública para editar en Inspector!
    public KeyCode dashKey = KeyCode.LeftShift;

    [Header("Dash Upgrades & Skills")]
    public bool canDashInvincibility = false; // Activa esto cuando el jugador obtenga el Power-Up
    public bool IsInvincible { get; private set; } // Propiedad para que otros scripts lean si eres invencible

    [Header("UI Settings")]
    public Image dashCooldownImage; // Arrastra aquí la imagen de la UI

    private float nextDash = 0f;
    private bool isDashing = false;
    private float dashTimer;

    // --- Movimiento ---
    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;

    // --- Flip ---
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        IsInvincible = false;
    }

    void Update()
    {
        // Actualizar la UI del Dash siempre
        UpdateDashUI();

        if (GameManagerUpdated.Instance.CurrentState != GameManagerUpdated.GameState.Gameplay)
            return; 

        // INPUT MOVIMIENTO
        moveInput = Input.GetAxisRaw("Horizontal");

        HandleFlip();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(dashKey) && Time.time > nextDash && !isDashing)
        {
            StartDash();
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (isDashing)
        {
            rb.linearVelocity = new Vector2((facingRight ? 1 : -1) * dashForce, 0);
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        ApplyBetterJumpGravity();
    }

    // --------------------------
    //      MÉTODOS PRINCIPALES
    // --------------------------

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void ApplyBetterJumpGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // --------------------------
    //      DASH & UI
    // --------------------------

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashTime;
        nextDash = Time.time + dashCooldown;
        rb.gravityScale = 0f;

        // Lógica de Invencibilidad
        if (canDashInvincibility)
        {
            IsInvincible = true;
            // Opcional: Aquí podrías cambiar el color del sprite para indicar invencibilidad
        }
    }

    void StopDash()
    {
        isDashing = false;
        rb.gravityScale = 1f;
        
        // Desactivar Invencibilidad
        IsInvincible = false;
    }

    void UpdateDashUI()
    {
        if (dashCooldownImage == null) return; // Evita errores si no asignaste la imagen

        if (Time.time > nextDash)
        {
            // El dash está listo
            dashCooldownImage.fillAmount = 1; 
        }
        else
        {
            // El dash está en enfriamiento (Cooldown)
            // Calculamos cuánto tiempo falta (de 0 a 1)
            float cooldownRemaining = nextDash - Time.time;
            float ratio = 1 - (cooldownRemaining / dashCooldown);
            dashCooldownImage.fillAmount = ratio;
        }
    }

    void LateUpdate()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                StopDash();
            }
        }
    }

    // --------------------------
    //      FLIP DEL PLAYER
    // --------------------------

    void HandleFlip()
    {
        if (moveInput > 0 && !facingRight) Flip(true);
        else if (moveInput < 0 && facingRight) Flip(false);
    }

    void Flip(bool faceRight)
    {
        facingRight = faceRight;
        float yRotation = faceRight ? 0f : 180f;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}