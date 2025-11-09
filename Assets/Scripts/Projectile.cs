using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Configuraci�n del Proyectil")]
    public float velocidad = 20f;
    public float tiempoDeVida = 2f; // Este valor define el ALCANCE
    // public float dano = 10f; // Para futuras mejoras

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = transform.right * velocidad;

        Destroy(gameObject, tiempoDeVida);
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Aqui ira la logica de dano al enemigo

        if (!hitInfo.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
