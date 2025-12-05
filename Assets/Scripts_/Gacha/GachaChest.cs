/*******************************************************
* Project: All Metal Drive
* Script: GachaChest.cs
* Author: Daniel C.
* Created: 18/11/2025
* Last Modified: 18/11/2025 by Daniel C.
*
* Description:
* Script for connecting the gacha menu to the physical chest
*
* Hours Worked: 0.5 code + 0.5 doco
*
* Dependencies:
* GachaMenu.cs
* UIMsg.cs
*
* Sections:
* - VARIABLES (agrupadas con [Header()])
* - METODOS PRINCIPALES
* - FUNCIONES AUXILIARES
* - EVENTOS UNITY
*
* Notes / Warnings:
* tweak the script if opening the same chest multiple times is intended
* gacha logic on GachaMenu.cs
* needs further tweaking to implement opening cost once an inventory has been done
*******************************************************/
using UnityEngine;
public class GachaChest : MonoBehaviour
{


    // ==================================================
    // ================ VARIABLES HEADER ================
    // ==================================================
    [Header("COST")]
    //token cost of opening the chest
    //public int cost=1;
    [Header("CONTROL")]
    //whether the player is in opening range
    bool interactable = false;
    //for avoiding opening the same chest twice
    bool opened = false;
    [Header("DEPENDENCIES")]
    //handles the gacha's menu on canvas
    // private GachaMenu gachaMenu;
    public GameObject gachaMenu;
    // public GameManagerUpdated gamemanagerUpdated;

    //shows messages on UI (canvas) (Replaceable by Debug.Log() )
    // private UIMsg uimsg;
    public UIMsg uimsg;

    // ==================================================
    // =============== MAIN METHODS ===============
    // ==================================================
    /// <summary>
    /// Finds and assigns necessary components.
    /// Main script functionality.
    /// </summary>

    void Start()
    {
        //gachaMenu=FindAnyObjectByType<GachaMenu>();
        //uimsg=FindAnyObjectByType<UIMsg>();
        interactable = false;
        opened = false;
    }
    void Update()
    {
        if (interactable && Input.GetKeyDown(KeyCode.E) && !opened)
        {
            Debug.Log("Opening Gacha Chest");
            //gachaMenu.ShowMenu();
            opened = true;

            // Si tienes un método para abrir el menú y asignar cartas, úsalo aquí:
            // gachaMenu.GetComponent<GachaMenuSO>().ShowMenu();
            // O si usas GachaMenuOpener:
            // gachaMenu.GetComponent<GachaMenuOpener>().OpenGachaMenu();

            gachaMenu.SetActive(true); // Solo activa el menú si no tienes un método específico
            var gachaMenuSO = gachaMenu.GetComponent<CardUpgradeGachaMenuSO>();
            if (gachaMenuSO != null) gachaMenuSO.SetupOption();

            // Pausa el juego a traves de GameManagerUpdated.
            GameManagerUpdated.Instance.ChangeState(GameManagerUpdated.GameState.Pause);
        }
    }


    // ==================================================
    // ============= AUXILIARY FUNCTIONS ===============
    // ==================================================
    /// <summary>
    /// Keeps track of interactability to avoid physics vs update framerate mismatch
    /// </summary>


    // --- VERSIÓN 3D ---
    /*
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            interactable=true;
            uimsg.Say("[E] Open"); //future devs swap this out for your system
        }
    }
    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            interactable = false;
        }
    }
    */

    // --- VERSIÓN 2D ---
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player") && !opened)
        {
            interactable = true;
            uimsg.Say("[E] Open"); //future devs swap this out for your system
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            interactable = false;
        }
    }


}