/**
* Project: All Metal Drive 
* Script: PlayerController.cs
* Author: Eduardo de Jesús Mancillas García
* Created: 11/16/2025
* Last Modified: 11/16/2025 by Eduardo Mancillas
*
* Description:
* Controls the player's 2.5D movement (2D physics) and combat.
* Manages movement, jumping, dash, and a 3-weapon swap system.
* Implements a "shooting mode" where movement is locked and movement keys
* are used to aim in 8 directions.
*
* Hours Worked: 2
*
* Dependencies:
* - Rigidbody2D (required on the same GameObject)
* - Projectile.cs (required on projectile prefabs)
* - A child "groundCheck" GameObject used to detect the ground.
* - A child "firePoint" GameObject used to instantiate projectiles.
*
* Notes / Warnings:
* - The player must be on a Layer that is NOT included
*   in the "groundLayer" mask for groundCheck to work.
* - Projectiles must be prefabs with the Projectile.cs script.
* - Horizontal movement is blocked while shooting.
********/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public KeyCode dashKey = KeyCode.LeftShift;

    private float nextDash = 0f;
    private bool isDashing = false;
    private float dashTime = 0.15f;
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
    }

    void Update()
    {

        if (GameManagerUpdated.Instance.CurrentState != GameManagerUpdated.GameState.Gameplay)
            return; // No permitir movimiento si no estamos en estado Gameplay

        // INPUT MOVIMIENTO
        moveInput = Input.GetAxisRaw("Horizontal");

        // FLIP DEL PERSONAJE
        HandleFlip();

        // SALTO
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // DASH
        if (Input.GetKeyDown(dashKey) && Time.time > nextDash && !isDashing)
        {
            StartDash();
        }
    }

    void FixedUpdate()
    {
        // DETECCIÓN DE PISO
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        );

        // SI ESTÁ HACIENDO DASH, IGNORA MOVIMIENTO NORMAL
        if (isDashing)
        {
            rb.linearVelocity = new Vector2((facingRight ? 1 : -1) * dashForce, 0);
            return;
        }

        // MOVIMIENTO HORIZONTAL
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // GRAVEDAD MEJORADA
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
    //      DASH
    // --------------------------

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashTime;
        nextDash = Time.time + dashCooldown;
        rb.gravityScale = 0f;
    }

    void StopDash()
    {
        isDashing = false;
        rb.gravityScale = 1f;
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
    //      FLIP DEL PLAYER With Ortographic Camera
    // --------------------------
    /* void HandleFlip()
    {
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; 
        transform.localScale = scale;
    } */

    // --------------------------
    //      FLIP DEL PLAYER With Perspective Camera
    // --------------------------

    void HandleFlip()
    {
        if (moveInput > 0 && !facingRight)
        {
            Flip(true);
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip(false);
        }
    }

    void Flip(bool faceRight)
    {
        facingRight = faceRight;

        // 0° si mira a derecha, 180° si mira a izquierda
        float yRotation = faceRight ? 0f : 180f;

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }


}
