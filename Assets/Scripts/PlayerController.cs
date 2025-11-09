using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Estado del Jugador")]
    public float vidaActual; // Para futuras mejoras
    public float vidaMaxima = 100f;

    [Header("Movimiento")]
    public float velocidadMovimiento = 7f;
    private float inputHorizontal;
    private bool estaMirandoDerecha = true;

    [Header("Salto")]
    public float fuerzaSalto = 16f;
    public Transform checkSuelo;
    public LayerMask capaSuelo;
    public float radioCheckSuelo = 0.2f;
    private bool estaEnSuelo;

    [Header("Dash")]
    public float velocidadDash = 20f;
    public float duracionDash = 0.2f;
    public float cooldownDash = 1f;
    private bool puedeDashear = true;
    private bool estaDasheando;

    [Header("Sistema de Disparo")]
    public Transform puntoDisparo;
    public Camera camaraJuego;
    public enum ModoDisparo { Medio, Escopeta, Largo }
    public ModoDisparo modoActual = ModoDisparo.Medio;
    private float tiempoUltimoDisparo;

    [Header("Disparo - Modo Medio")]
    public GameObject proyectilMedio;
    public float cadenciaMedio = 0.5f;

    [Header("Disparo - Modo Escopeta")]
    public GameObject proyectilEscopeta;
    public float cadenciaEscopeta = 1f;
    public int cantidadProyectilesEscopeta = 5;
    public float anguloDispersionEscopeta = 15f; // angulo total de dispersion

    [Header("Disparo - Modo Largo")]
    public GameObject proyectilLargo;
    public float cadenciaLargo = 2f;

    // Componentes y referencias
    private Rigidbody2D rb;
    private Vector2 direccionDisparo;
    private Vector2 posicionMouse;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        vidaActual = vidaMaxima;
    }

    void Update()
    {
        if (estaDasheando)
        {
            return;
        }

        // INPUTS
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        // Input de Salto
        if (Input.GetButtonDown("Jump") && estaEnSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }

        // Input de Dash
        if (Input.GetButtonDown("Fire3") && puedeDashear)
        {
            StartCoroutine(Dash());
        }

        // Input de Disparo
        if (Input.GetButtonDown("Fire1"))
        {
            IntentarDisparar();
        }

        // Input para cambiar de arma (Ej: Teclas 1, 2, 3)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            modoActual = ModoDisparo.Medio;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            modoActual = ModoDisparo.Escopeta;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            modoActual = ModoDisparo.Largo;
        }

        estaEnSuelo = Physics2D.OverlapCircle(checkSuelo.position, radioCheckSuelo, capaSuelo);

        posicionMouse = camaraJuego.ScreenToWorldPoint(Input.mousePosition);

        // Voltear al personaje basado en la posicion del mouse
        if (posicionMouse.x > transform.position.x && !estaMirandoDerecha)
        {
            Voltear();
        }
        else if (posicionMouse.x < transform.position.x && estaMirandoDerecha)
        {
            Voltear();
        }
    }

    void FixedUpdate()
    {
        if (estaDasheando)
        {
            return;
        }

        rb.linearVelocity = new Vector2(inputHorizontal * velocidadMovimiento, rb.linearVelocity.y);
    }

    private void IntentarDisparar()
    {
        float cadenciaActual = 0f;

        switch (modoActual)
        {
            case ModoDisparo.Medio:
                cadenciaActual = cadenciaMedio;
                break;
            case ModoDisparo.Escopeta:
                cadenciaActual = cadenciaEscopeta;
                break;
            case ModoDisparo.Largo:
                cadenciaActual = cadenciaLargo;
                break;
        }

        if (Time.time >= tiempoUltimoDisparo + cadenciaActual)
        {
            Disparar();
            tiempoUltimoDisparo = Time.time;
        }
    }

    private void Disparar()
    {
        direccionDisparo = (posicionMouse - (Vector2)puntoDisparo.position).normalized;
        float angulo = Mathf.Atan2(direccionDisparo.y, direccionDisparo.x) * Mathf.Rad2Deg;
        Quaternion rotacionProyectil = Quaternion.Euler(0f, 0f, angulo);

        switch (modoActual)
        {
            case ModoDisparo.Medio:
                Instantiate(proyectilMedio, puntoDisparo.position, rotacionProyectil);
                break;

            case ModoDisparo.Escopeta:
                for (int i = 0; i < cantidadProyectilesEscopeta; i++)
                {
                    float anguloDisperso = angulo + Random.Range(-anguloDispersionEscopeta / 2, anguloDispersionEscopeta / 2);
                    Quaternion rotacionDispersa = Quaternion.Euler(0f, 0f, anguloDisperso);
                    Instantiate(proyectilEscopeta, puntoDisparo.position, rotacionDispersa);
                }
                break;

            case ModoDisparo.Largo:
                Instantiate(proyectilLargo, puntoDisparo.position, rotacionProyectil);
                break;
        }
    }

    private IEnumerator Dash()
    {
        puedeDashear = false;
        estaDasheando = true;
        float gravedadOriginal = rb.gravityScale;
        rb.gravityScale = 0f; // Ignorar gravedad durante el dash

        float direccionDash = estaMirandoDerecha ? 1f : -1f;
        rb.linearVelocity = new Vector2(direccionDash * velocidadDash, 0f);

        yield return new WaitForSeconds(duracionDash);

        estaDasheando = false;
        rb.gravityScale = gravedadOriginal;
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(cooldownDash);
        puedeDashear = true;
    }

    private void Voltear()
    {
        estaMirandoDerecha = !estaMirandoDerecha;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmosSelected()
    {
        if (checkSuelo != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(checkSuelo.position, radioCheckSuelo);
        }
    }
}