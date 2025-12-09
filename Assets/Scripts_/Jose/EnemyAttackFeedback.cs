using UnityEngine;

public class EnemyAttackFeedback : MonoBehaviour
{
    [Header("Efecto de ataque")]
    [SerializeField] private GameObject attackEffectPrefab;
    [SerializeField] private Transform attackEffectPoint;

    public void PlayAttackEffect()
    {
        if (attackEffectPrefab == null) return;
        Vector3 spawnPos = attackEffectPoint != null ? attackEffectPoint.position : transform.position;
        GameObject effect = Instantiate(
            attackEffectPrefab,
            spawnPos,
            Quaternion.identity
        );
    }
}
