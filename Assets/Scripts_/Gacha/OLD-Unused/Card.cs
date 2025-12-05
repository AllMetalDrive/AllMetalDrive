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
* - METODOS PRINCIPALES
* - FUNCIONES AUXILIARES
* - EVENTOS UNITY
*
* Notes / Warnings:
* (none)
*******************************************************/
using UnityEngine;
public abstract class Card : MonoBehaviour
{


// ==================================================
// ================ VARIABLES HEADER ================
// ==================================================
[Header("COMPONENTS")]
//the card's name
public string cardName="Card";
//the card's image
public Sprite img;

// ==================================================
// =============== MAIN METHODS ===============
// ==================================================
/// <summary>
/// Each card template (class child) has its own functionality,
/// this is to call it in general.
/// </summary>

public abstract void Action();
}