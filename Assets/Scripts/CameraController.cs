/**
* Project: All Metal Drive
* Script: CameraController.cs
* Author: Eduardo de Jesús Mancillas García
* Created: 16/11/2025
* Last Modified: 16/11/2025 by Eduardo Mancillas
*
* Description:
* Controla la cámara principal para que siga al jugador.
* Utiliza un método de interpolación (Lerp) en LateUpdate para un
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
* jugador haya completado su movimiento de física (FixedUpdate) y
* lógica (Update) del frame actual.
********/

using UnityEngine;

public class CameraController : MonoBehaviour // 
{
    // =======================================================
    // =================== VARIABLES HEADER ==================
    // =======================================================

    [Header("CONFIGURACIÓN DE SEGUIMIENTO")]
    // El Transform del objeto que la cámara debe seguir (el jugador)
    public Transform playerTransform;

    // Velocidad del suavizado.
    // Un valor más bajo = más suave y lento.
    // Un valor más alto = más rápido y "duro".
    public float followSpeed = 5f;

    // El desplazamiento (offset) deseado de la cámara respecto al jugador.
    // Para 2.5D, ajusta Z a (ej: -10) para alejar la cámara.
    // Ajusta Y (ej: 2) si quieres que la cámara esté un poco por encima.
    public Vector3 offset;

    // =======================================================
    // =================== EVENTOS UNITY =====================
    // =======================================================

    /// <summary>
    /// Se llama cada frame, DESPUÉS de que todos los Updates se hayan ejecutado.
    /// Ideal para lógica de cámaras para evitar tirones (jitter).
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
    /// Calcula la posición deseada y la interpola suavemente.
    /// </summary>
    private void FollowPlayer()
    {
        // 1. Calcular la posición objetivo
        // (Posición del jugador + el desplazamiento definido)
        Vector3 desiredPosition = playerTransform.position + offset;

        // 2. Interpolar suavemente desde la posición actual hacia la deseada
        // Usamos Lerp (Interpolación Lineal)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // 3. Aplicar la nueva posición a la cámara
        transform.position = smoothedPosition;
    }
}