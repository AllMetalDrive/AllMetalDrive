using UnityEngine;

public class FakeShadow : MonoBehaviour
{
    public Transform target;
    public LayerMask groundLayer;

    public float maxDistance = 3f;
    public float maxScale = 1f;
    public float minScale = 0.3f;

    void Update()
    {
        if (target == null) return;

        // Raycast 2D hacia abajo
        RaycastHit2D hit = Physics2D.Raycast(
            target.position,
            Vector2.down,
            maxDistance,
            groundLayer
        );

        // Debug visual
        Debug.DrawRay(target.position, Vector2.down * maxDistance, Color.red);

        if (hit.collider != null)
        {
            // Posicionar la sombra en el suelo
            transform.position = new Vector3(
                hit.point.x,
                hit.point.y + 0.01f,
                transform.position.z
            );

            // Escalar según altura
            float t = hit.distance / maxDistance;
            float scale = Mathf.Lerp(maxScale, minScale, t);
            transform.localScale = Vector3.one * scale;
        }
    }
}
