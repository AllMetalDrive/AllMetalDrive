using UnityEngine;

public class FakeShadow : MonoBehaviour
{
    public Transform target;   // Jugador
    public float heightOffset = 0.01f;
    public float maxScale = 1f;
    public float minScale = 0.3f;
    
    void Update()
    {
        // Seguir posición (solo XZ si estás en 3D)
        transform.position = new Vector3(
            target.position.x,
            transform.position.y,
            target.position.z
        );

        // Cambiar tamaño según altura del jugador
        float height = target.position.y;
        float t = Mathf.InverseLerp(2f, 0f, height); // Ajusta este rango
        float scale = Mathf.Lerp(minScale, maxScale, t);

        transform.localScale = new Vector3(scale, scale, scale);
    }
}
