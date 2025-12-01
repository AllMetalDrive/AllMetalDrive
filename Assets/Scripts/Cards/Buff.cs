using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "All Metal Drive/Buff")]
public class Buff : ScriptableObject
{
    [Header("Buff Information")]
    public string buffName;
    [TextArea]
    public string description;
    
    [Header("Stat Modifiers")]
    public float moveSpeedMultiplier = 1f;
    public float jumpForceMultiplier = 1f;
    public float dashSpeedMultiplier = 1f;
    public float damageMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public float healthMultiplier = 1f;
    
    [Header("Duration")]
    public bool isPermanent = true;
    public float duration = 0f; // Solo si no es permanente
}