/*******************************************************
* Project: All Metal Drive
* Script: PlayerUpgradeManager.cs
* Author: [Tu Nombre]
* Created: [Fecha]
*
* Description:
* Gestiona todas las mejoras aplicadas al jugador
*******************************************************/

using UnityEngine;
using System.Collections.Generic;

public class PlayerUpgradeManager : MonoBehaviour
{
    [Header("REFERENCES")]
    public PlayerController playerController;

    [Header("ACTIVE UPGRADES")]
    public List<CardUpgradeSO> activeUpgrades = new List<CardUpgradeSO>();

    // Stats acumulados
    private Dictionary<string, float> statBonuses = new Dictionary<string, float>();
    private Dictionary<string, float> statMultipliers = new Dictionary<string, float>();
    private Dictionary<string, bool> specialAbilities = new Dictionary<string, bool>();

    void Start()
    {
        InitializeDictionaries();
        FindPlayerController();
    }

    void InitializeDictionaries()
    {
        statBonuses.Clear();
        statMultipliers.Clear();
        specialAbilities.Clear();

        // Inicializar stats base
        statBonuses.Add("moveSpeed", 0f);
        statBonuses.Add("jumpForce", 0f);
        statBonuses.Add("dashSpeed", 0f);
        statBonuses.Add("dashCooldown", 0f);

        statMultipliers.Add("damage", 1f);
        statMultipliers.Add("fireRate", 1f);
        statMultipliers.Add("projectileSpeed", 1f);
        statMultipliers.Add("lifetime", 1f);

        specialAbilities.Add("doubleJump", false);
        specialAbilities.Add("airDash", false);
        specialAbilities.Add("piercingShots", false);
        specialAbilities.Add("explosiveShots", false);
    }

    void FindPlayerController()
    {
        if (playerController == null)
        {
            playerController = FindAnyObjectByType<PlayerController>();
        }
    }

    public void ApplyUpgrade(CardUpgradeSO upgrade)
    {
        if (!CanApplyUpgrade(upgrade)) return;

        activeUpgrades.Add(upgrade);
        CalculateTotalStats();
        ApplyStatsToPlayer();

        Debug.Log($"Upgrade aplicado: {upgrade.upgradeName}");
    }

    bool CanApplyUpgrade(CardUpgradeSO upgrade)
    {
        if (!upgrade.isStackable)
        {
            // Contar cuántas veces ya tenemos este upgrade
            int count = 0;
            foreach (var activeUpgrade in activeUpgrades)
            {
                if (activeUpgrade == upgrade) count++;
            }

            if (count >= upgrade.maxStacks) return false;
        }

        return true;
    }

    void CalculateTotalStats()
    {
        InitializeDictionaries();

        foreach (var upgrade in activeUpgrades)
        {
            // Sumar bonuses
            statBonuses["moveSpeed"] += upgrade.moveSpeedBonus;
            statBonuses["jumpForce"] += upgrade.jumpForceBonus;
            statBonuses["dashSpeed"] += upgrade.dashSpeedBonus;
            statBonuses["dashCooldown"] += upgrade.dashCooldownReduction;

            // Multiplicar multiplicadores
            statMultipliers["damage"] *= upgrade.damageMultiplier;
            statMultipliers["fireRate"] *= upgrade.fireRateMultiplier;
            statMultipliers["projectileSpeed"] *= upgrade.projectileSpeedMultiplier;
            statMultipliers["lifetime"] *= upgrade.lifetimeMultiplier;

            // Activar habilidades especiales
            if (upgrade.doubleJump) specialAbilities["doubleJump"] = true;
            if (upgrade.airDash) specialAbilities["airDash"] = true;
            if (upgrade.piercingShots) specialAbilities["piercingShots"] = true;
            if (upgrade.explosiveShots) specialAbilities["explosiveShots"] = true;
        }
    }

    void ApplyStatsToPlayer()
    {
        if (playerController == null) return;

        // Aquí aplicarías los stats al PlayerController
        // Nota: Necesitarías agregar métodos públicos en PlayerController para esto
    }

    public float GetStatBonus(string statName)
    {
        return statBonuses.ContainsKey(statName) ? statBonuses[statName] : 0f;
    }

    public float GetStatMultiplier(string statName)
    {
        return statMultipliers.ContainsKey(statName) ? statMultipliers[statName] : 1f;
    }

    public bool HasSpecialAbility(string abilityName)
    {
        return specialAbilities.ContainsKey(abilityName) && specialAbilities[abilityName];
    }
}