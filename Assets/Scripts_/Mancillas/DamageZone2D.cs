/**
* Project: All Metal Drive
* Script: DamageZone2D.cs
* Author: Adaptado por GitHub Copilot
* Created: 30/11/2025
*
* Description:
* Gestiona un área estática que aplica daño en el tiempo (DoT) al jugador en 2D.
* Se autodestruye después de un tiempo definido.
*
* Dependencies:
* - Collider2D con IsTrigger activo.
* - PlayerHealth (script externo del jugador, referencia comentada).
*/

using UnityEngine;

public class DamageZone2D : MonoBehaviour
{
    [Header("DURACIÓN Y VIDA")]
    public float duration = 3f;

    [Header("SISTEMA DE DAÑO")]
    public float damagePerSecond = 5f;
    public float damageTickRate = 0.5f;

    private float _timer;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _timer += Time.deltaTime;
            if (_timer >= damageTickRate)
            {
                ApplyDamage(other.gameObject);
                _timer = 0;
            }
        }
    }

    private void ApplyDamage(GameObject target)
    {
        // TODO: Conectar con el sistema de vida real del jugador
        // var healthComponent = target.GetComponent<PlayerHealth>();
        // if(healthComponent != null) healthComponent.TakeDamage(damagePerSecond);
        Debug.Log($"Jugador quemándose: Recibiendo daño en la zona de fuego (2D).");
    }
}
