using UnityEngine;

public class AimAtMousePerspective : MonoBehaviour
{
    void Update()
    {
        // Creamos un rayo desde la cámara hacia el mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Definimos un plano donde "vive" tu personaje.
        // Suponemos que tus personajes están en Z = 0
        Plane plane = new Plane(Vector3.forward, Vector3.zero);

        float distance;

        // ¿El rayo intersecta el plano?
        if (plane.Raycast(ray, out distance))
        {
            // Punto de impacto del rayo en el plano del personaje
            Vector3 hitPoint = ray.GetPoint(distance);

            // Dirección desde tu objeto hacia el punto donde apuntas
            Vector3 direction = hitPoint - transform.position;

            // Calcula el ángulo en grados
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Aplica la rotación solo en el eje Z
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
