/*******************************************************
* Project: [All Metal Drive]
* Script: AudioManager.cs
* Author: José Cruz
* Created: 16/11/2025
* Last Modified: 17/11/2025 by José Cruz
*
* Description:
* Administrador centralizado de audio para reproducir 
* efectos de sonido del jugador, enemigos y otros sistemas.
* Usa un sistema de pool de AudioSources para permitir
* múltiples sonidos simultáneos sin interferencias.
*
* Hours Worked: 2
*
* Dependencies:
* - AudioSource
* - Controladores que llamen sus métodos 
*   (PlayerHealthFeedback, EnemyHealthFeedback, ataques, etc.)
*
* Sections:
* - VARIABLES
* - MÉTODOS PRINCIPALES
* - FUNCIONES AUXILIARES
* - EVENTOS UNITY
*
* Notes / Warnings:
* - Asignar todos los clips manualmente en el Inspector.
* - Evitar reproducir clips nulos.
*******************************************************/

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // ==================================================
    // ===================== VARIABLES ===================
    // ==================================================

    [Header("CATEGORIAS DE AUDIO")]
    // Categorías lógicas para expandir organización futura.
    // Ejemplos: COMBATE, ENTORNO, UI.
    // No contienen variables directamente; solo sirven como guía en el inspector.


    [Header("AUDIO SOURCE PRINCIPAL")]
    [SerializeField] private AudioSource sfxSource;    // Fuente principal de audio (fallback / debugging)


    [Header("CLIPS JUGADOR")]
    [SerializeField] private AudioClip playerDamageClip;   // Sonido al recibir daño
    [SerializeField] private AudioClip playerHealClip;     // Sonido al curarse
    [SerializeField] private AudioClip playerDeathClip;    // Sonido al morir
    [SerializeField] private AudioClip playerShootClip;    // Sonido al disparar


    [Header("CLIPS ENEMIGO")]
    [SerializeField] private AudioClip enemyDamageClip;    // Sonido al recibir daño
    [SerializeField] private AudioClip enemyHealClip;      // Sonido al curarse
    [SerializeField] private AudioClip enemyDeathClip;     // Sonido al morir
    [SerializeField] private AudioClip enemyAttackClip;    // Sonido al disparar / atacar


    [Header("POOL DE AUDIOSOURCES")]
    [SerializeField] private int poolSize = 5;             // Cantidad de fuentes en el pool
    private AudioSource[] sfxPool;                         // Arreglo de AudioSources reutilizables
    private int poolIndex = 0;                             // Índice rotatorio para selección de AudioSource



    // ==================================================
    // =============== MÉTODOS PRINCIPALES ==============
    // ==================================================

    /// <summary>Reproduce sonido cuando el jugador recibe daño.</summary>
    public void PlayerTakeDamage() => PlayClip(playerDamageClip);

    /// <summary>Reproduce sonido cuando el jugador se cura.</summary>
    public void PlayerHeal() => PlayClip(playerHealClip);

    /// <summary>Reproduce sonido cuando el jugador muere.</summary>
    public void PlayerDeath() => PlayClip(playerDeathClip);

    /// <summary>Reproduce sonido cuando el jugador dispara.</summary>
    public void PlayerShoot() => PlayClip(playerShootClip);


    // ---Efectos de sonido de enemigos---
    

    /// <summary>Reproduce sonido cuando un enemigo recibe daño.</summary>
    public void EnemyTakeDamage() => PlayClip(enemyDamageClip);

    /// <summary>Reproduce sonido cuando un enemigo se cura.</summary>
    public void EnemyHeal() => PlayClip(enemyHealClip);

    /// <summary>Reproduce sonido cuando un enemigo muere.</summary>
    public void EnemyDeath() => PlayClip(enemyDeathClip);

    /// <summary>Reproduce sonido cuando un enemigo dispara/ataca.</summary>
    public void EnemyShoot() => PlayClip(enemyAttackClip);



    // ==================================================
    // ============= FUNCIONES AUXILIARES ===============
    // ==================================================

    /// <summary>
    /// Reproduce un AudioClip usando el sistema de pool.
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        PlayClip(clip);
    }

    /// <summary>
    /// Mecanismo interno para reproducir un clip usando un pool circular.
    /// Garantiza reproducir múltiples sonidos sin cortarse entre sí.
    /// </summary>
    private void PlayClip(AudioClip clip)
    {
        if (clip == null || sfxPool == null || sfxPool.Length == 0)
            return;

        AudioSource source = sfxPool[poolIndex];
        poolIndex = (poolIndex + 1) % sfxPool.Length;

        source.PlayOneShot(clip);
    }



    // ==================================================
    // =================== EVENTOS UNITY ================
    // ==================================================

    /// <summary>
    /// Inicializa el AudioSource principal y genera el pool
    /// de AudioSources secundarios para reproducción simultánea.
    /// </summary>
    private void Awake()
    {
        if (sfxSource == null)
            sfxSource = GetComponent<AudioSource>();

        // Crear el pool de AudioSources
        sfxPool = new AudioSource[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            GameObject newSourceObj = new GameObject($"SFX_Source_{i}");
            newSourceObj.transform.parent = this.transform;

            AudioSource newSource = newSourceObj.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            newSource.spatialBlend = 0f; // Sonido 100% 2D
            sfxPool[i] = newSource;
        }
    }
}
