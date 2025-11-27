/*******************************************************
* Project: All Metal Drive
* Script: UpgradeCard.cs
* Author: [Tu Nombre]
* Created: [Fecha]
*
* Description:
* Carta que aplica mejoras al jugador
*******************************************************/

using UnityEngine;

public class UpgradeCard : Card
{
    public CardUpgradeSO upgradeData;
    private PlayerUpgradeManager upgradeManager;

    void Start()
    {
        upgradeManager = FindAnyObjectByType<PlayerUpgradeManager>();
        if (upgradeManager == null)
        {
            Debug.LogError("UpgradeManager no encontrado en la escena!");
        }
    }

    public override void Action()
    {
        if (upgradeManager != null && upgradeData != null)
        {
            upgradeManager.ApplyUpgrade(upgradeData);
            ShowUpgradeUI();
        }
    }

    void ShowUpgradeUI()
    {
        // Aquí llamarías a tu UI para mostrar la mejora aplicada
        UpgradeUIManager uiManager = FindAnyObjectByType<UpgradeUIManager>();
        if (uiManager != null)
        {
            uiManager.ShowUpgradeNotification(upgradeData);
        }
    }
}