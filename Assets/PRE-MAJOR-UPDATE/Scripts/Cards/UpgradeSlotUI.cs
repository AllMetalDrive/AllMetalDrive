/*******************************************************
* Project: All Metal Drive
* Script: UpgradeSlotUI.cs
* Author: [Tu Nombre]
* Created: [Fecha]
*
* Description:
* UI individual para cada mejora en el menú
*******************************************************/

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeSlotUI : MonoBehaviour
{
    [Header("UI ELEMENTS")]
    public Image upgradeIcon;
    public TMP_Text upgradeNameText;
    public TMP_Text upgradeDescriptionText;
    public TMP_Text upgradeStatsText;
    public GameObject rarityStars;

    public void SetUpgrade(CardUpgradeSO upgrade)
    {
        upgradeIcon.sprite = upgrade.icon;
        upgradeNameText.text = upgrade.upgradeName;
        upgradeDescriptionText.text = upgrade.description;

        // Mostrar stats
        string stats = "";
        if (upgrade.moveSpeedBonus != 0) stats += $"Vel: +{upgrade.moveSpeedBonus} ";
        if (upgrade.jumpForceBonus != 0) stats += $"Salto: +{upgrade.jumpForceBonus} ";
        if (upgrade.damageMultiplier != 1f) stats += $"Daño: x{upgrade.damageMultiplier}";

        upgradeStatsText.text = stats;

        // Mostrar rareza
        UpdateRarityDisplay(upgrade.rarity);
    }

    void UpdateRarityDisplay(int rarity)
    {
        if (rarityStars == null) return;

        // Activar estrellas según la rareza
        for (int i = 0; i < rarityStars.transform.childCount; i++)
        {
            rarityStars.transform.GetChild(i).gameObject.SetActive(i < rarity);
        }
    }
}