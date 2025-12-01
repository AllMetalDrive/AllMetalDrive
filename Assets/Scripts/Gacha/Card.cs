/*******************************************************
* Project: All Metal Drive
* Script: Card.cs
* Author: Daniel C.
* Created: 18/11/2025
* Last Modified: 18/11/2025 by Daniel C.
*
* Description:
* A template for card templates
*
* Hours Worked: 0.016 code + 0.05 doco
*
* Dependencies:
* (none)
*
* Sections:
* - VARIABLES (agrupadas con [Header()])
* - M�TODOS PRINCIPALES
* - FUNCIONES AUXILIARES
* - EVENTOS UNITY
*
* Notes / Warnings:
* (none)
*******************************************************/
using UnityEngine;
public abstract class Card : MonoBehaviour
{
    [Header("COMPONENTS")]
    public string cardName = "Card";
    public Sprite img;
    
    [Header("BUFF")]
    public Buff cardBuff;  // El ScriptableObject del buff

    public abstract void Action();
    
    // Nuevo metodo para obtener el buff
    public Buff GetBuff()
    {
        return cardBuff;
    }
}