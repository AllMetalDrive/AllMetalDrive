/* using UnityEngine;
using System.Collections.Generic;

public class BuffManager : MonoBehaviour
{
    [Header("Active Buffs")]
    public List<Buff> activeBuffs = new List<Buff>();
    
    private PlayerController playerController;
    private GameManager gameManager; // Si tienes un game manager

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        // gameManager = FindObjectOfType<GameManager>();
    }

    public void ApplyBuff(Buff buff)
    {
        if (buff == null)
        {
            Debug.LogWarning("Intento de aplicar un buff nulo");
            return;
        }
        
        activeBuffs.Add(buff);
        UpdatePlayerStats();
        
        Debug.Log($"Buff aplicado: {buff.buffName}");
        
        // Si el buff no es permanente, programar su remoción
        if (!buff.isPermanent)
        {
            StartCoroutine(RemoveBuffAfterTime(buff, buff.duration));
        }
    }

    private System.Collections.IEnumerator RemoveBuffAfterTime(Buff buff, float duration)
    {
        yield return new WaitForSeconds(duration);
        RemoveBuff(buff);
    }

    public void RemoveBuff(Buff buff)
    {
        if (activeBuffs.Contains(buff))
        {
            activeBuffs.Remove(buff);
            UpdatePlayerStats();
            Debug.Log($"Buff removido: {buff.buffName}");
        }
    }

    private void UpdatePlayerStats()
    {
        if (playerController == null) return;
        
        // Resetear a valores base primero
        playerController.ResetToBaseStats();
        
        // Aplicar todos los buffs acumulativos
        foreach (Buff buff in activeBuffs)
        {
            playerController.ApplyBuffModifiers(buff);
        }
    }
    
    // Método para obtener estadísticas totales
    public float GetTotalMoveSpeedMultiplier()
    {
        float total = 1f;
        foreach (Buff buff in activeBuffs)
        {
            total *= buff.moveSpeedMultiplier;
        }
        return total;
    }
} */