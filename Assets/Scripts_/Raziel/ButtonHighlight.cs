/*******************************************************
* Project: All Metal Drive
* Script: ButtonHighlight.cs
* Author: Raziel
* Created: 18/11/2025
*
* Description:
* Handles mouse-over highlighting for UI buttons.
* Technical artists can later assign custom sprites
* or visual effects.
*******************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;

    [Header("HIGHLIGHT COLORS")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightColor = Color.yellow;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = normalColor;
    }
}