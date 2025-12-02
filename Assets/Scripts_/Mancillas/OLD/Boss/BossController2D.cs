/**
* Project: All Metal Drive
* Script: BossController2D.cs
* Author: Adaptado por GitHub Copilot
* Created: 30/11/2025
*
* Description:
* Controlador avanzado para el jefe "Dragón" en 2D.
* Implementa una máquina de estados para vuelo, posicionamiento y 3 tipos de ataques especiales.
* Gestiona animaciones y restricciones de arena en 2D.
*
* Dependencies:
* - Rigidbody2D (IsKinematic = true)
* - BoxCollider2D para la Arena.
* - Animator.
*/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class BossController2D : MonoBehaviour
{
    [Header("ESTADÍSTICAS")]
    public float maxHealth = 100f;
    public float moveSpeed = 8f;
    public float attackCooldown = 2f;

    [Header("VUELO IDLE")]
    public float hoverHeight = 5f;
    public float movementSmoothing = 2f;

    [Header("ATAQUE: BOLAS DE FUEGO")]
    public GameObject fireballPrefab;
    public Transform mouthFirePoint;
    public float fireballBurstDelay = 0.3f;

    [Header("ATAQUE: EMBESTIDA (DASH)")]
    public float dashSpeed = 25f;
    public float dashPrepareTime = 1f;

    [Header("ATAQUE: CAÍDA (SLAM)")]
    public float slamSpeed = 30f;
    public float slamRecoveryTime = 1.5f;

    [Header("REFERENCIAS")]
    public Transform playerTransform;
    public BoxCollider2D arenaBoundary;
    public Animator animator;

    private Rigidbody2D _rb;
    private bool _isActivated = false;
    private bool _isAttacking = false;
    private float _attackTimer;

    private readonly int _animIsFlying = Animator.StringToHash("isFlying");
    private readonly int _animAttackFireball = Animator.StringToHash("attackFireball");
    private readonly int _animAttackDash = Animator.StringToHash("attackDash");
    private readonly int _animAttackSlam = Animator.StringToHash("attackSlam");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true;
    }

    private void Start()
    {
        animator.SetBool(_animIsFlying, true);
    }

    private void Update()
    {
        if (playerTransform == null) return;
        if (!_isActivated)
        {
            float distance = Vector2.Distance(transform.position, playerTransform.position);
            if (distance <= 15f)
            {
                _isActivated = true;
                _attackTimer = attackCooldown;
            }
            return;
        }
        if (_isAttacking) return;
        HandleIdleMovement();
        HandleCombatLogic();
    }

    private void HandleIdleMovement()
    {
        float targetY = arenaBoundary.bounds.min.y + hoverHeight;
        Vector2 targetPos = new Vector2(playerTransform.position.x, targetY);
        targetPos = GetClampedPosition(targetPos);
        Vector2 smoothedPos = Vector2.Lerp(transform.position, targetPos, movementSmoothing * Time.deltaTime);
        _rb.MovePosition(smoothedPos);
        FacePlayer();
    }

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

    private IEnumerator FireballAttackRoutine()
    {
        _isAttacking = true;
        animator.SetTrigger(_animAttackFireball);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 3; i++)
        {
            if (fireballPrefab != null && mouthFirePoint != null)
            {
                GameObject fireball = Instantiate(fireballPrefab, mouthFirePoint.position, Quaternion.identity);
                Vector2 direction = (playerTransform.position - mouthFirePoint.position).normalized;
                // BossProjectile projScript = fireball.GetComponent<BossProjectile>();
                BossProjectile2D projScript = fireball.GetComponent<BossProjectile2D>();

                if (projScript != null) projScript.SetDirection(direction);
            }
            yield return new WaitForSeconds(fireballBurstDelay);
        }
        yield return new WaitForSeconds(1f);
        _isAttacking = false;
        animator.SetBool(_animIsFlying, true);
    }

    private IEnumerator HorizontalDashAttackRoutine()
    {
        _isAttacking = true;
        bool startFromLeft = playerTransform.position.x > arenaBoundary.bounds.center.x;
        float startX = startFromLeft ? arenaBoundary.bounds.min.x + 2f : arenaBoundary.bounds.max.x - 2f;
        float endX = startFromLeft ? arenaBoundary.bounds.max.x - 2f : arenaBoundary.bounds.min.x + 2f;
        Vector2 startPos = new Vector2(startX, playerTransform.position.y);
        yield return StartCoroutine(MoveToPosition(startPos, 1.5f));
        FacePoint(new Vector2(endX, transform.position.y));
        animator.SetBool(_animIsFlying, true);
        yield return new WaitForSeconds(dashPrepareTime);
        animator.SetTrigger(_animAttackDash);
        float time = 0;
        Vector2 initialDashPos = transform.position;
        Vector2 targetDashPos = new Vector2(endX, transform.position.y);
        while (time < 1f)
        {
            time += Time.deltaTime * (dashSpeed / 10f);
            _rb.MovePosition(Vector2.Lerp(initialDashPos, targetDashPos, time));
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        _isAttacking = false;
    }

    private IEnumerator VerticalSlamAttackRoutine()
    {
        _isAttacking = true;
        Vector2 hoverPos = new Vector2(playerTransform.position.x, arenaBoundary.bounds.max.y - 1f);
        hoverPos = GetClampedPosition(hoverPos);
        yield return StartCoroutine(MoveToPosition(hoverPos, 1.0f));
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger(_animAttackSlam);
        Vector2 groundPos = new Vector2(transform.position.x, arenaBoundary.bounds.min.y + 1f);
        while (Vector2.Distance(transform.position, groundPos) > 0.5f)
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, groundPos, slamSpeed * Time.deltaTime);
            _rb.MovePosition(newPos);
            yield return null;
        }
        yield return new WaitForSeconds(slamRecoveryTime);
        animator.SetBool(_animIsFlying, true);
        _isAttacking = false;
    }

    private IEnumerator MoveToPosition(Vector2 target, float duration)
    {
        Vector2 startPos = transform.position;
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime / duration;
            _rb.MovePosition(Vector2.Lerp(startPos, target, time));
            if (target.x > transform.position.x) transform.localScale = new Vector3(1, 1, 1);
            else transform.localScale = new Vector3(-1, 1, 1);
            yield return null;
        }
    }

    private Vector2 GetClampedPosition(Vector2 pos)
    {
        if (arenaBoundary == null) return pos;
        Bounds b = arenaBoundary.bounds;
        return new Vector2(
            Mathf.Clamp(pos.x, b.min.x, b.max.x),
            Mathf.Clamp(pos.y, b.min.y, b.max.y)
        );
    }

    private void FacePlayer()
    {
        if (playerTransform.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void FacePoint(Vector2 point)
    {
        if (point.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isAttacking && collision.gameObject.CompareTag("Player"))
        {
            // Lógica de daño al tocar al jugador
            // collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(20);
            Debug.Log("¡Jefe golpeó al jugador con el cuerpo!");
        }
    }
}
