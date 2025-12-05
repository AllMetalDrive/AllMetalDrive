/**
* Project: All Metal Drive
* Script: CameraController.cs
* Author: Eduardo de Jesus Mancillas Garcia
* Created: 16/11/2025
* Last Modified: 16/11/2025 by Eduardo Mancillas
*
* Description:
* Controla la camara principal para que siga al jugador.
* Utiliza un metodo de interpolacion (Lerp) en LateUpdate para un
* seguimiento suave y evitar "jittering".
* Mantiene un "offset" (desplazamiento) constante respecto al jugador.
*
* Hours Worked: 0.5
*
* Dependencies:
* - El GameObject del Jugador debe existir en la escena y tener un Transform.
*
* Sections:
* - VARIABLES
* - EVENTOS UNITY
*
* Notes / Warnings:
* - Este script debe ejecutarse en LateUpdate() para asegurar que el
* jugador haya completado su movimiento de fisica (FixedUpdate) y
* logica (Update) del frame actual.
********/

using UnityEngine;

public class CameraController : MonoBehaviour // 
{
    // =======================================================
    // =================== VARIABLES HEADER ==================
    // =======================================================

    [Header("CONFIGURACION DE SEGUIMIENTO")]
    // El Transform del objeto que la camara debe seguir (el jugador)
    public Transform playerTransform;

    // Velocidad del suavizado.
    // Un valor mas bajo = mas suave y lento.
    // Un valor mas alto = mas rapido y "duro".
    public float followSpeed = 5f;

    // El desplazamiento (offset) deseado de la camara respecto al jugador.
    // Para 2.5D, ajusta Z a (ej: -10) para alejar la camara.
    // Ajusta Y (ej: 2) si quieres que la camara este un poco por encima.
    public Vector3 offset;

    // =======================================================
    // =================== EVENTOS UNITY =====================
    // =======================================================

    /// <summary>
    /// Se llama cada frame, DESPUES de que todos los Updates se hayan ejecutado.
    /// Ideal para logica de camaras para evitar tirones (jitter).
    /// </summary>
    private void LateUpdate()
    {
        // Si no hay jugador asignado, no hacer nada (previene errores)
        if (playerTransform == null)
        {
            Debug.LogWarning("Referencia del 'playerTransform' no asignada en el CameraController.");
            return;
        }

        FollowPlayer();
    }

    // =======================================================
    // ================ FUNCIONES AUXILIARES =================
    // =======================================================

    /// <summary>
    /// Calcula la posicion deseada y la interpola suavemente.
    /// </summary>
    private void FollowPlayer()
    {
        // 1. Calcular la posicion objetivo
        // (Posicion del jugador + el desplazamiento definido)
        Vector3 desiredPosition = playerTransform.position + offset;

        // 2. Interpolar suavemente desde la posicion actual hacia la deseada
        // Usamos Lerp (Interpolacion Lineal)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // 3. Aplicar la nueva posicion a la camara
        transform.position = smoothedPosition;
    }
}