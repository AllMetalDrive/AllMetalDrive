/*******************************************************
* Project: All Metal Drive
* Script: Card.cs
* Author: Daniel C.
* Created: 19/10/2025
* Last Modified: 19/11/2025 by Daniel C.
*
* Description:
* Shows messages on UI
*
* Hours Worked: 0.016 code + 0.05 doco
*
* Dependencies:
* TextMeshPro
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
using TMPro;
public abstract class UIMsg : MonoBehaviour
{


// ==================================================
// ================ VARIABLES HEADER ================
// ==================================================
[Header("COMPONENTS")]
//the message textbox
public TMP_Text message;
[Header("CLEARING")]
//how long the message lasts onscreen (CD)
public float lifetime = 5f;
//ICD for clearing the message
float cleartime;
//whether or not the shown message is persistent
bool canClear = true;

// ==================================================
// =============== MAIN METHODS ===============
// ==================================================
/// <summary>
/// Main functionality.
/// </summary>

void Start()
{
    ClearMsg();
}
void Update()
{
    if(canClear&&Time.time>cleartime){
        ClearMsg();
    }
}

public void SayPersistent(string msg){
    canClear=false;
    message.text=msg;
}
public void Say(string msg){
    canClear=true;
    cleartime=Time.time+lifetime;
    message.text=msg;
}
public void ClearMsg(){
    canClear=false;
    message.text="";
}

}