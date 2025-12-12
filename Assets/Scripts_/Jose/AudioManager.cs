/*******************************************************
* Project: [All Metal Drive]
* Script: AudioManager.cs
* Author: José Cruz
* Created: 16/11/2025
* Last Modified: 10/12/2025 by Rodrigo Garcia de Quevedo
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
* - SINGLETON (nuevo)
* - VARIABLES
* - MÉTODOS PRINCIPALES
* - FUNCIONES AUXILIARES
* - MÚSICA DE FONDO
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
    // ==================== SINGLETON ====================
    // ==================================================
    /*
     * Esta sección permite acceder al AudioManager desde 
     * cualquier script con AudioManager.Instance.
     * Necesario para que GameManager pueda iniciar música.
     */
     
    public static AudioManager Instance { get; private set; }



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
    [SerializeField] private AudioClip playerDamageClip;
    [SerializeField] private AudioClip playerHealClip;
    [SerializeField] private AudioClip playerDeathClip;
    [SerializeField] private AudioClip playerShootClip;


    [Header("CLIPS ENEMIGO")]
    [SerializeField] private AudioClip enemyDamageClip;
    [SerializeField] private AudioClip enemyHealClip;
    [SerializeField] private AudioClip enemyDeathClip;
    [SerializeField] private AudioClip enemyAttackClip;


    [Header("POOL DE AUDIOSOURCES")]
    [SerializeField] private int poolSize = 5;
    private AudioSource[] sfxPool;
    private int poolIndex = 0;



    // ==================================================
    // =============== MÉTODOS PRINCIPALES ==============
    // ==================================================

    public void PlayerTakeDamage() => PlayClip(playerDamageClip);
    public void PlayerHeal() => PlayClip(playerHealClip);
    public void PlayerDeath() => PlayClip(playerDeathClip);
    public void PlayerShoot() => PlayClip(playerShootClip);

    public void EnemyTakeDamage() => PlayClip(enemyDamageClip);
    public void EnemyHeal() => PlayClip(enemyHealClip);
    public void EnemyDeath() => PlayClip(enemyDeathClip);
    public void EnemyShoot() => PlayClip(enemyAttackClip);



    // ==================================================
    // ============= FUNCIONES AUXILIARES ===============
    // ==================================================

    public void PlaySFX(AudioClip clip) => PlayClip(clip);

    private void PlayClip(AudioClip clip)
    {
        if (clip == null || sfxPool == null || sfxPool.Length == 0) return;

        AudioSource source = sfxPool[poolIndex];
        poolIndex = (poolIndex + 1) % sfxPool.Length;
        source.PlayOneShot(clip);
    }



    // ==================================================
    // ================== MÚSICA DE FONDO ===============
    // ==================================================

    [Header("MÚSICA DE FONDO")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip gameplayMusicClip;

    /// <summary>Reproduce música general asignada al inspector.</summary>
    public void PlayMusic()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.spatialBlend = 0f;
        }

        if (gameplayMusicClip == null) return;

        musicSource.clip = gameplayMusicClip;
        musicSource.Play();
    }

    /// <summary>Permite recibir música desde GameManager.</summary>
    public void PlayMusic(AudioClip clip) // <-- Método que faltaba
    {
        if (clip == null) return;

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.spatialBlend = 0f;
        }

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null) musicSource.Stop();
    }



    // ==================================================
    // =================== EVENTOS UNITY ================
    // ==================================================

    private void Awake()
    {
        // ---- SINGLETON ----
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);


        // ---- SFX POOL ----
        if (sfxSource == null) sfxSource = GetComponent<AudioSource>();

        sfxPool = new AudioSource[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = new GameObject($"SFX_Source_{i}");
            obj.transform.parent = this.transform;

            AudioSource src = obj.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.spatialBlend = 0f;
            sfxPool[i] = src;
        }
    }
}
