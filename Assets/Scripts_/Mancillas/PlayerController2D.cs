/**
* Project: All Metal Drive 
* Script: PlayerController2D.cs
* Author: Eduardo de Jesús Mancillas García
* Created: 11/16/2025
* Last Modified: 12/08/2025
*
* Description:
* Controls the player's 2.5D movement, combat, and UI feedback.
* Includes jump anticipation animation with movement lock.
********/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Dash Settings")]
    public float dashForce = 18f;
    public float dashCooldown = 1.2f;
    public float dashTime = 0.15f;
    public KeyCode dashKey = KeyCode.LeftShift;

    [Header("Dash Upgrades & Skills")]
    public bool canDashInvincibility = false;
    public bool IsInvincible { get; private set; }

    [Header("UI Settings")]
    public Image dashCooldownImage;

    // --- Dash ---
    private float nextDash = 0f;
    private bool isDashing = false;
    private float dashTimer;

    // --- Movimiento ---
    private Rigidbody2D rb;
    private float moveInput;

    // --- Flip ---
    private bool facingRight = true;

    // --- Animator ---
    public Animator animator;

    [Header("References")]
    public GroundCheck groundCheck;

    // =====================================================
    // NUEVO: Estados para salto con anticipación
    // =====================================================
    private bool isPreparingJump = false;   // Bloquea movimiento y flip
    private bool jumpRequested = false;     // Evita múltiples requests

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        IsInvincible = false;
    }

    void Update()
    {
        UpdateDashUI();

        if (GameManagerUpdated.Instance.CurrentState != GameManagerUpdated.GameState.Gameplay)
            return;

        // INPUT MOVIMIENTO
        moveInput = Input.GetAxisRaw("Horizontal");

        HandleFlip();

        // =====================================================
        // NUEVO: Ya NO saltamos aquí, solo activamos animación
        // =====================================================
        if (Input.GetButtonDown("Jump") && groundCheck.IsGrounded && !jumpRequested)
        {
            jumpRequested = true;
            isPreparingJump = true;                 // Bloquea movimiento
            animator.SetTrigger("prepareJump");    // Animación anticipación
        }

        if (Input.GetKeyDown(dashKey) && Time.time > nextDash && !isDashing)
        {
            StartDash();
        }
    }

    void FixedUpdate()
    {
        animator.SetBool("isGrounded", groundCheck.IsGrounded);

        if (isDashing)
        {
            rb.linearVelocity = new Vector2(
                (facingRight ? 1 : -1) * dashForce,
                0
            );
            return;
        }

        // =====================================================
        // NUEVO: Bloqueo de movimiento horizontal
        // durante la animación PrepareJump
        // =====================================================
        if (!isPreparingJump)
        {
            rb.linearVelocity = new Vector2(
                moveInput * moveSpeed,
                rb.linearVelocity.y
            );
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        bool isRunning = Mathf.Abs(moveInput) > 0.1f;
        animator.SetBool("isRunning", isRunning);

        ApplyBetterJumpGravity();
    }

    // =====================================================
    // MÉTODO LLAMADO DESDE ANIMATION EVENT
    // (al final de PrepareJump)
    // =====================================================
    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        animator.SetBool("isJumping", true);

        // NUEVO: Liberamos el bloqueo
        isPreparingJump = false;
        jumpRequested = false;
    }

    void ApplyBetterJumpGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up *
                Physics2D.gravity.y *
                (fallMultiplier - 1) *
                Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up *
                Physics2D.gravity.y *
                (lowJumpMultiplier - 1) *
                Time.fixedDeltaTime;
        }
    }

    // --------------------------
    // DASH & UI
    // --------------------------

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashTime;
        nextDash = Time.time + dashCooldown;
        rb.gravityScale = 0f;

        if (canDashInvincibility)
            IsInvincible = true;
    }

    void StopDash()
    {
        isDashing = false;
        rb.gravityScale = 1f;
        IsInvincible = false;
    }

    void UpdateDashUI()
    {
        if (dashCooldownImage == null) return;

        if (Time.time > nextDash)
            dashCooldownImage.fillAmount = 1;
        else
        {
            float cooldownRemaining = nextDash - Time.time;
            dashCooldownImage.fillAmount =
                1 - (cooldownRemaining / dashCooldown);
        }
    }

    void LateUpdate()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
                StopDash();
        }

        // NUEVO: Reset al aterrizar
        if (groundCheck.IsGrounded)
            animator.SetBool("isJumping", false);
    }

    // --------------------------
    // FLIP
    // --------------------------

    void HandleFlip()
    {
        // =====================================================
        // NUEVO: Evita flip durante PrepareJump
        // =====================================================
        if (isPreparingJump) return;

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
