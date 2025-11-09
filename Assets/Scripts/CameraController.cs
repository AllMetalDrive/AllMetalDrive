using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Objetivo a Seguir")]
    public Transform target;

    [Header("Configuración de Cámara")]
    [Tooltip("Qué tan suavemente sigue la cámara. Un valor bajo (ej. 0.1) es rápido, un valor alto (ej. 0.5) es más suave.")]
    public float smoothTime = 0.3f;

    [Tooltip("La distancia de la cámara al jugador. Para 2D, Z debe ser negativo (ej. -10).")]
    public Vector3 offset = new Vector3(0, 0, -10);

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;

            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            transform.position = smoothedPosition;
        }
    }
}