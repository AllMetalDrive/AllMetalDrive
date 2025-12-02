/**
* Project: All Metal Drive
* Script: DamageZone.cs
* Author: Eduardo de Jesus Mancillas Garcia
* Created: 28/11/2025
* Last Modified: 28/11/2025 by Eduardo de Jesus Mancillas Garcia
*
* Description:
* Gestiona un área estática que aplica daño en el tiempo (DoT) al jugador.
* Se autodestruye después de un tiempo definido.
*
* Hours Worked: 0.5h
*
* Dependencies:
* - Collider (3D) con IsTrigger activo.
* - PlayerHealth (script externo del jugador, referencia comentada).
*
* Sections:
* - VARIABLES HEADER
* - EVENTOS UNITY
*
* Notes / Warnings:
* - Asegurarse de que el Collider tenga marcada la casilla IsTrigger.
********/

using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // =======================================================
    // =================== VARIABLES HEADER ==================
    // =======================================================

    [Header("DURACIÓN Y VIDA")]
    // Tiempo en segundos antes de que la zona desaparezca
    public float duration = 3f; 

    [Header("SISTEMA DE DAÑO")]
    // Daño total por segundo (referencial)
    public float damagePerSecond = 5f; 
    // Frecuencia con la que se aplica el daño (segundos)
    public float damageTickRate = 0.5f; 

    // --- Variables Privadas ---
    private float _timer;

    // =======================================================
    // =================== EVENTOS UNITY =====================
    // =======================================================

    /// <summary>
    /// Configura la autodestrucción del objeto.
    /// </summary>
    private void Start()
    {
        Destroy(gameObject, duration);
    }

    /// <summary>
    /// Aplica daño mientras el jugador permanezca dentro del trigger.
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Sistema de Tick para no aplicar daño en cada frame
            _timer += Time.deltaTime;
            
            if (_timer >= damageTickRate)
            {
                ApplyDamage(other.gameObject);
                _timer = 0;
            }
        }
    }

    // =======================================================
    // ================= FUNCIONES AUXILIARES ================
    // =======================================================

    /// <summary>
    /// Ejecuta la lógica de aplicar daño al objetivo.
    /// </summary>
    /// <param name="target">El objeto que recibirá el daño.</param>
    private void ApplyDamage(GameObject target)
    {
        // TODO: Conectar con el sistema de vida real del jugador
        // var healthComponent = target.GetComponent<PlayerHealth>();
        // if(healthComponent != null) healthComponent.TakeDamage(damagePerSecond);
        
        Debug.Log($"Jugador quemándose: Recibiendo daño en la zona de fuego.");
    }
}