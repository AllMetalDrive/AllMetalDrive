/*******************************************************
* Project: [Nombre del Proyecto]
* Script: EnemyHealthFeedback.cs
* Author: José Cruz
* Created: [16/11/2025]
* Last Modified: [17/11/2025] by José Cruz
*
* Description:
* Maneja el feedback visual y sonoro del sistema de salud del enemigo.
* Utiliza el AudioManager para reproducir sonidos centralizados y
* genera efectos visuales al recibir daño, curarse o morir.
*
* Hours Worked: [1]
*
* Dependencies:
* - EnemyHealth.cs
* - AudioManager.cs
* - Prefabs de efectos visuales
*
* Sections:
* - VARIABLES
* - MÉTODOS PRINCIPALES
* - FUNCIONES AUXILIARES
* - EVENTOS UNITY
*
* Notes / Warnings:
* - Asegurarse de asignar los efectos visuales requeridos.
* - Los sonidos ya no deben asignarse aquí; se gestionan desde AudioManager.
* - Contiene la misma estructura que PlayerHealthFeedback.cs. Considerar
*   la posibilidad de una clase base común si se repite más código.
*******************************************************/

using UnityEngine;
using System.Collections;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class EnemyHealthFeedback : MonoBehaviour
{
    // ======================= VARIABLES =======================

    [Header("REFERENCIAS")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Material dissolveMaterial;
    private Material _runtimeMat;


    [Header("EFECTOS VISUALES")]
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private GameObject healEffect;
    [SerializeField] private GameObject deathEffect;

    [Header("CONFIGURACIÓN DE EFECTOS")]
    [SerializeField] private float effectLifetime = 2f; // Tiempo antes de destruir efectos visuales
    [SerializeField] private float dissolveDuration = 1.5f;
    private bool isDissolving = false;

    //public MMF_Player mMF_Player;
    public MMProgressBar mMProgressBar;

    // ======================= UI / HEALTH BINDING =======================

    private void OnEnable()
    {
        if (enemyHealth == null)
            enemyHealth = GetComponent<EnemyHealth>();

        if (enemyHealth != null)
            enemyHealth.OnHealthChanged += HandleHealthChanged;
    }

    private void OnDisable()
    {
        if (enemyHealth != null)
            enemyHealth.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(int current, int max)
    {
        if (mMProgressBar == null)
            return;

        float normalized = (max <= 0) ? 0f : (float)current / max;
        mMProgressBar.UpdateBar01(normalized);
    }

    // ======================= MÉTODOS PRINCIPALES =======================

    /// <summary>
    /// Feedback al recibir daño.
    /// </summary>
    public void OnDamageFeedback()
    {
        audioManager?.EnemyTakeDamage();
        //TriggerEffect(damageEffect);
    }

    /// <summary>
    /// Feedback al recibir curación.
    /// </summary>
    public void OnHealFeedback()
    {
        audioManager?.EnemyHeal();
        //TriggerEffect(healEffect);
    }

    /// <summary>
    /// Feedback al morir.
    /// </summary>
    public void OnDeathFeedback()
    {
        audioManager?.EnemyDeath();
        //TriggerEffect(deathEffect);

        StartCoroutine(DissolveEffect());
        // Activar animacion de muerte o efectos adicionales aquí si es necesario
    }


    // ======================= FUNCIONES AUXILIARES =======================

    private void TriggerEffect(GameObject effectPrefab)
    {
        if (effectPrefab == null) return;

        GameObject instance = Instantiate(
            effectPrefab,
            transform.position,
            Quaternion.identity
        );

        if (effectLifetime > 0f)
            Destroy(instance, effectLifetime);
    }

    // Callback para notificar cuando termina el efecto de disolución
    public System.Action OnDissolveComplete;

    // Efecto de disolución al morir (requiere shader adecuado)
    private IEnumerator DissolveEffect()
    {
        isDissolving = true;

        // Asignar el material de disolución al renderer
        var renderer = GetComponentInChildren<Renderer>();
        if (renderer != null && dissolveMaterial != null)
        {
            // Instanciar el material para no afectar a otros enemigos
            _runtimeMat = new Material(dissolveMaterial);
            renderer.material = _runtimeMat;

            float t = 0f;
            while (t < dissolveDuration)
            {
                float dissolveAmount = Mathf.Lerp(0f, 1f, t / dissolveDuration);
                _runtimeMat.SetFloat("_DissolveAmount", dissolveAmount);
                t += Time.deltaTime;
                yield return null;
            }
            _runtimeMat.SetFloat("_DissolveAmount", 1f);
        }

        // Notificar a EnemyHealth que terminó el efecto
        OnDissolveComplete?.Invoke();
    }


    // ======================= EVENTOS UNITY =======================

    private void Awake()
    {
        if (enemyHealth == null)
            enemyHealth = GetComponent<EnemyHealth>();

        if (audioManager == null)
            audioManager = FindAnyObjectByType<AudioManager>();
    }
}
