/*******************************************************
* Project: [All Metal Drive]
* Script: HealthBase.cs
* Author: José Cruz
* Created: 15/11/2025
* Last Modified: 08/12/2025 by Rodrigo Garcia de Quevedo
*
* Description:
* Clase base encargada de manejar salud, daño y curación.
* Es heredada por PlayerHealth y EnemyHealth para 
* definir comportamientos específicos de cada entidad.
*
* Hours Worked: [3.5]
*
* Dependencies:
* - Clases hijas (PlayerHealth, EnemyHealth)
* - Sistemas de combate (Projectile, MeleeAttack, etc.)
*
* Sections:
* - VARIABLES
* - EVENTOS UNITY
* - MÉTODOS PRINCIPALES
* - FUNCIONES AUXILIARES
*
* Notes / Warnings:
* - Las clases hijas deben implementar HandleDeath().
* - currentHealth se reinicia automáticamente en Start().
*******************************************************/

using UnityEngine;
using System; // necesario para eventos

public abstract class HealthBase : MonoBehaviour
{
    // ==================================================
    // ===================== VARIABLES ===================
    // ==================================================

    [Header("SALUD BASE")]
    [SerializeField] private protected int maxHealth = 100;      // Salud máxima del objeto
    [SerializeField] private protected int currentHealth = 100;  // Salud actual del objeto

    [Header("CONFIGURACIÓN DE DAÑO")]
    [SerializeField] protected bool canTakeDamage = true;        // Indica si puede recibir daño


    // ================== EVENTOS ==================
    /// <summary>
    /// Evento que notifica cambios de salud: (current, max)
    /// </summary>
    public event Action<int, int> OnHealthChanged;

    /// <summary>
    /// Evento que notifica la muerte de la entidad.
    /// </summary>
    public event Action OnDeathEvent;



    // ==================================================
    // =================== EVENTOS UNITY ================
    // ==================================================

    /// <summary>
    /// Inicializa la salud actual al valor máximo.
    /// </summary>
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        // Notificar UI/oyentes del valor inicial
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }



    // ==================================================
    // =============== MÉTODOS PRINCIPALES ==============
    // ==================================================

    /// <summary>
    /// Aplica daño a la salud de la entidad.
    /// </summary>
    /// <param name="amount">Cantidad de daño.</param>
    public virtual void TakeDamage(int amount)
    {
        if (!canTakeDamage)
            return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"{gameObject.name} recibió {amount} daño. Salud actual: {currentHealth}");

        // Notificar cambios (UI, sonidos, etc.)
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        CheckHealthState();
    }

    /// <summary>
    /// Recupera salud en la cantidad indicada.
    /// </summary>
    /// <param name="amount">Cantidad a curar.</param>
    public virtual void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"{gameObject.name} se curó {amount}. Salud actual: {currentHealth}");

        // Notificar cambios (UI, sonidos, etc.)
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }



    // ==================================================
    // ============= FUNCIONES AUXILIARES ===============
    // ==================================================

    /// <summary>
    /// Verifica si la salud ha llegado a cero.
    /// </summary>
    protected virtual void CheckHealthState()
    {
        if (currentHealth <= 0)
        {
            // Notificar muerte antes de ejecutar lógica de muerte
            OnDeathEvent?.Invoke();
            HandleDeath();
        }
    }

    /// <summary>
    /// Acción ejecutada cuando la salud llega a cero.
    /// Debe implementarse en las clases hijas.
    /// </summary>
    protected abstract void HandleDeath();
}
