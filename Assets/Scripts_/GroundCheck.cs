using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Ground Check Settings")]
    public float radius = 0.2f;
    public LayerMask groundLayer;

    public bool IsGrounded { get; private set; }

    void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(
            transform.position,
            radius,
            groundLayer
        );
    }

    // =============================
    // VISUALIZACIÓN EN SCENE
    // =============================
    void OnDrawGizmosSelected()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
