using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardSlotUI : MonoBehaviour
{
    [Header("UI Components")]
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text descText;

    // Asigna los datos del ScriptableObject a los componentes UI
    public void SetCard(CardUpgradeSO upgrade)
    {
        if (iconImage != null) iconImage.sprite = upgrade.icon;
        if (nameText != null) nameText.text = upgrade.upgradeName;
        if (descText != null) descText.text = upgrade.description;
    }
}
