/*******************************************************
* Project: All Metal Drive
* Script: CardUpgradeSO.cs
* Author: [Tu Nombre]
* Created: [Fecha]
*
* Description:
* Scriptable Object para definir mejoras de cartas
*******************************************************/

using UnityEngine;

[CreateAssetMenu(fileName = "New Card Upgrade", menuName = "All Metal Drive/Card Upgrade")]
public class CardUpgradeSO : ScriptableObject
{
    [Header("CARD INFO")]
    public string upgradeName;
    public string description;
    public Sprite icon;
    public int rarity; // 3, 4, 5 estrellas

    [Header("STAT UPGRADES")]
    public float moveSpeedBonus = 0f;
    public float jumpForceBonus = 0f;
    public float dashSpeedBonus = 0f;
    public float dashCooldownReduction = 0f;

    [Header("WEAPON UPGRADES")]
    public float damageMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public float projectileSpeedMultiplier = 1f;
    public float lifetimeMultiplier = 1f;

    [Header("SPECIAL ABILITIES")]
    public bool doubleJump = false;
    public bool airDash = false;
    public bool piercingShots = false;
    public bool explosiveShots = false;

    [Header("STACKING")]
    public bool isStackable = true;
    public int maxStacks = 1;
}