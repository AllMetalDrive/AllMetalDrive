/*******************************************************
* Project: All Metal Drive
* Script: UpgradeUIManager.cs
* Author: [Tu Nombre]
* Created: [Fecha]
*
* Description:
* Maneja la UI de mejoras del jugador
*******************************************************/

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{
    [Header("UPGRADE NOTIFICATION")]
    public GameObject upgradeNotificationPanel;
    public Image upgradeIcon;
    public TMP_Text upgradeNameText;
    public TMP_Text upgradeDescriptionText;
    public TMP_Text statChangesText;

    [Header("UPGRADE MENU")]
    public GameObject upgradesMenuPanel;
    public Transform upgradesGridParent;
    public GameObject upgradeSlotPrefab;

    [Header("STATS DISPLAY")]
    public TMP_Text moveSpeedText;
    public TMP_Text jumpForceText;
    public TMP_Text dashStatsText;
    public TMP_Text weaponStatsText;
    public TMP_Text abilitiesText;

    private PlayerUpgradeManager upgradeManager;

    void Start()
    {
        upgradeManager = FindAnyObjectByType<PlayerUpgradeManager>();
        HideUpgradeNotification();
        HideUpgradesMenu();
    }

    void Update()
    {
        // Toggle del menú de mejoras
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradesMenu();
        }
    }

    public void ShowUpgradeNotification(CardUpgradeSO upgrade)
    {
        if (upgradeNotificationPanel == null) return;

        upgradeNotificationPanel.SetActive(true);
        upgradeIcon.sprite = upgrade.icon;
        upgradeNameText.text = upgrade.upgradeName;
        upgradeDescriptionText.text = upgrade.description;

        // Mostrar cambios de stats
        string statChanges = "";
        if (upgrade.moveSpeedBonus != 0) statChanges += $"Velocidad: +{upgrade.moveSpeedBonus}\n";
        if (upgrade.jumpForceBonus != 0) statChanges += $"Salto: +{upgrade.jumpForceBonus}\n";
        if (upgrade.damageMultiplier != 1f) statChanges += $"Daño: x{upgrade.damageMultiplier}\n";

        statChangesText.text = statChanges;

        // Ocultar automáticamente después de 3 segundos
        Invoke("HideUpgradeNotification", 3f);
    }

    public void HideUpgradeNotification()
    {
        if (upgradeNotificationPanel != null)
            upgradeNotificationPanel.SetActive(false);
    }

    public void ToggleUpgradesMenu()
    {
        if (upgradesMenuPanel.activeSelf)
        {
            HideUpgradesMenu();
        }
        else
        {
            ShowUpgradesMenu();
        }
    }

    public void ShowUpgradesMenu()
    {
        upgradesMenuPanel.SetActive(true);
        UpdateUpgradesDisplay();
        UpdateStatsDisplay();

        // Pausar el juego
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideUpgradesMenu()
    {
        upgradesMenuPanel.SetActive(false);

        // Reanudar el juego
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void UpdateUpgradesDisplay()
    {
        // Limpiar grid existente
        foreach (Transform child in upgradesGridParent)
        {
            Destroy(child.gameObject);
        }

        // Llenar con mejoras activas
        if (upgradeManager != null)
        {
            foreach (var upgrade in upgradeManager.activeUpgrades)
            {
                GameObject slot = Instantiate(upgradeSlotPrefab, upgradesGridParent);
                UpgradeSlotUI slotUI = slot.GetComponent<UpgradeSlotUI>();

                if (slotUI != null)
                {
                    slotUI.SetUpgrade(upgrade);
                }
            }
        }
    }

    void UpdateStatsDisplay()
    {
        if (upgradeManager == null) return;

        moveSpeedText.text = $"Velocidad: +{upgradeManager.GetStatBonus("moveSpeed")}";
        jumpForceText.text = $"Fuerza de Salto: +{upgradeManager.GetStatBonus("jumpForce")}";
        dashStatsText.text = $"Velocidad Dash: +{upgradeManager.GetStatBonus("dashSpeed")}";

        // Mostrar habilidades especiales
        string abilities = "Habilidades:\n";
        if (upgradeManager.HasSpecialAbility("doubleJump")) abilities += "• Doble Salto\n";
        if (upgradeManager.HasSpecialAbility("airDash")) abilities += "• Dash Aéreo\n";
        if (upgradeManager.HasSpecialAbility("piercingShots")) abilities += "• Disparos Penetrantes\n";

        abilitiesText.text = abilities;
    }
}