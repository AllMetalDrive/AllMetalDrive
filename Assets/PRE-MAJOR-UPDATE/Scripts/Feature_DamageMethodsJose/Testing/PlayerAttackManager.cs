/*******************************************************
* Project: [Nombre del Proyecto]
* Script: PlayerAttackManager.cs
* Author: [Nombre del Desarrollador]
* Created: [DD/MM/AAAA]
* Last Modified: [DD/MM/AAAA] by [Nombre del último editor]
*
* Description:
* Gestiona el sistema básico de ataque del jugador.
* Se encarga de instanciar proyectiles desde un punto
* de disparo, controlando el tiempo entre disparos.
*
* Hours Worked: [Total de horas aproximadas dedicadas]
*
* Dependencies:
* - Prefab del proyectil
*
* Sections:
* - VARIABLES
* - MÉTODOS PRINCIPALES
* - FUNCIONES AUXILIARES
* - EVENTOS UNITY
*
* Notes / Warnings:
* - Requiere asignar un prefab de proyectil.
* - Requiere asignar un punto de disparo (Transform).
* - Requiere activar el Input Manager (OLD) para "Fire1".
*******************************************************/

using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    // ======================= VARIABLES =======================

    [Header("REFERENCIAS")]
    [SerializeField] private GameObject projectilePrefab; // Prefab del proyectil a disparar
    [SerializeField] private Transform firePoint;         // Punto desde el cual se instancian los proyectiles

    [Header("ATAQUE")]
    [SerializeField] private float fireRate = 0.25f;      // Tiempo entre disparos
    private float nextFireTime = 0f;                      // Control del enfriamiento de disparo


    // ======================= MÉTODOS PRINCIPALES =======================

    /// <summary>
    /// Llamado para ejecutar un disparo del jugador.
    /// </summary>
    public void Shoot()
    {
        if (!CanShoot()) 
            return;

        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        nextFireTime = Time.time + fireRate;
    }


    // ======================= FUNCIONES AUXILIARES =======================

    private bool CanShoot()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("PlayerAttackManager: No se ha asignado el prefab del proyectil.");
            return false;
        }

        if (firePoint == null)
        {
            Debug.LogWarning("PlayerAttackManager: No se ha asignado el firePoint.");
            return false;
        }

        return Time.time >= nextFireTime;
    }


    // ======================= EVENTOS UNITY =======================

    private void Update()
    {
        // Ejemplo básico: disparar con clic izquierdo o botón principal
        if (Input.GetButton("Fire1"))
        {
            Shoot();
        }
    }
}
