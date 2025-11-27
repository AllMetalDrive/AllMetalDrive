/*******************************************************
* Project: All Metal Drive
* Script: GachaMenu.cs
* Author: Daniel C.
* Created: 18/11/2025
* Last Modified: 20/11/2025 by Daniel C.
*
* Description:
* Handles showing Gacha's menu and gacha logic.
* Includes multiple options for easy swapping of game design.
*
* Hours Worked: 11h code + 0.75h doco
*
* Dependencies:
* Card.cs
* UnityEngine.UI
* TextMeshPro
*
* Sections:
* - VARIABLES (agrupadas con [Header()])
* - M�TODOS PRINCIPALES
* - FUNCIONES AUXILIARES
* - EVENTOS UNITY
*
* Notes / Warnings:
* Please assign all required components.
* Works with any number of card options.
* MAKE SURE INDEXES FOR ALL COMPONENTS MATCH
* PITY STARTS AT 1, counted AFTER each pull.
* if sharedRarity is active, a pull is counted for all cards (once per menu call);
* if inactive, a pull is counted per card instead
*******************************************************/
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GachaMenu : MonoBehaviour
{


// ==================================================
// ================ VARIABLES HEADER ================
// ==================================================
[Header("CARDS")]
//holds all the 5* cards that exist
public Card[] prizes5s;
//holds all the 4* cards that exist
public Card[] prizes4s;
//holds all the 3* cards that exist
public Card[] prizes3s;
[Header("PROBABILITIES")]
//sets probability of getting a 5*
public float probability5s=0.01f;
//sets probability of getting a 4*
public float probability4s=1.0f;
//3s prob is remaining prob
[Header("PITY")]
//only matters if hasPity is active
//sets pity to reach to guarantee a 5*
public int pity5s=90;
//sets pity to reach to guarantee a 4*
public int pity4s=10;
//keeps track of how many non-5* have been won (how long till pity)
int count5s=1;
//keeps track of how many non-4* (optionally or better) have been won (how long till pity)
int count4s=1;
[Header("OPTIONS")]
//whether losing enough times guarantees a 5* or 4* card
public bool hasPity=false; //fate moment
//whether winning a 4* also resets 5* pity
public bool alsoReset4s=true; //crying emogi
//whether winning a specific rarity means all slots are of that rarity
public bool sharedRarity=false;
//whether the same card can appear as different options
public bool allowDupes=false;
[Header("COMPONENTS")]
//the actual menu
//should be a panel on canvas with a title text and slots with buttons,
//each with an image and a text, representing each card option;
//clicking the button calls the card's action, and resumes the game
public GameObject menu;
//holds all the images, set size on inspector to match actual options
//informs the script how many card option slots exist
public Image[] cardImgs;
//holds all the texts
public TMP_Text[] cardTxs;
[Header("CARD ASSIGNMENT")]
//saves rarities for each option slot
int[] rarities;
//saves indexes for each option slot,
//used after knowing rarity (informs which rarity array to pull from)
int[] prizeIdx;
//actual card options
Card[] cards;


// ==================================================
// =============== MAIN METHODS ===============
// ==================================================
/// <summary>
/// Finds and assigns necessary components.
/// Main script functionality.
/// </summary>

void Start(){
    if(cardImgs.Length!=cardTxs.Length){
        Debug.Log("Warning: GachaMenu: Card Images and Texts count must be equal!");
    }else if(cardImgs.Length==0||cardTxs.Length==0||menu==null){
        Debug.Log("Warning: GachaMenu: Please assign all components!");
    }else{
        rarities=new int[cardImgs.Length];
        prizeIdx=new int[cardImgs.Length];
        cards=new Card[cardImgs.Length];
    }
    HideMenu();
}

void SetupOption(){
    if(sharedRarity){
        int rarity=CalculateRarity();
        for(int i=0; i<rarities.Length; i++){
            rarities[i]=rarity;
        }
    }else{
        for(int i=0; i<rarities.Length; i++){
            rarities[i]=CalculateRarity();
        }
    }
    if(!allowDupes){
        bool isValid=false;
        while(!isValid){
            GenerateIndex();
            //validation
            isValid=true;
            int i=0;
            int j=0;
            while(i<prizeIdx.Length){
                j=i+1;
                while(j<prizeIdx.Length){
                    if(prizeIdx[i]==prizeIdx[j] && rarities[i]==rarities[j]){
                        isValid=false;
                    }
                    j++;
                }//j loop
                i++;
            }//i loop
        }//validator
    }else{
        GenerateIndex();
    }
    for(int i=0; i<prizeIdx.Length; i++){
        switch(rarities[i]){
            case 5:
                cardImgs[i].sprite=prizes5s[i].img;
                cardTxs[i].text=prizes5s[i].cardName;
                cards[i]=prizes5s[i];
                break;
            case 4:
                cardImgs[i].sprite=prizes4s[i].img;
                cardTxs[i].text=prizes4s[i].cardName;
                cards[i]=prizes4s[i];
                break;
            default:
                cardImgs[i].sprite=prizes3s[i].img;
                cardTxs[i].text=prizes3s[i].cardName;
                cards[i]=prizes3s[i];
                break;
        }
    }
}


// ==================================================
// ============= AUXILIARY FUNCTIONS ===============
// ==================================================
/// <summary>
/// Rote operations
/// </summary>

void GenerateIndex(){
    for(int i=0; i<prizeIdx.Length; i++){
        switch(rarities[i]){
            case 5:
                prizeIdx[i]=Random.Range(0, prizes5s.Length);
                break;
            case 4:
                prizeIdx[i]=Random.Range(0, prizes4s.Length);
                break;
            default:
                prizeIdx[i]=Random.Range(0, prizes3s.Length);
                break;
        }
    }
}
int CalculateRarity(){
    int rarity;
    if(hasPity && count5s>=pity5s){
        rarity=5;
        CountPity(5);
    }else if(hasPity && count4s>=pity4s){
        rarity=4;
        CountPity(4);
    }else{
        float probability=Random.Range(0.001f, 99.999f);
        if(probability<=probability5s){
            rarity=5;
            CountPity(5);
        }else if(probability<=probability4s+probability5s){
            rarity=4;
            CountPity(4);
        }else{
            rarity=3;
            CountPity(3);
        }
    }
    return rarity;
}
void CountPity(int wonRarity){
    switch(wonRarity){
        case 5:
            count5s=1;
            if(alsoReset4s){
                count4s=1;
            }else{
                count4s++;
            }
            break;
        case 4:
            count4s=1;
            count5s++;
            break;
        default:
            count5s++;
            count4s++;
            break;
    }
}

public void ShowMenu(){
    menu.SetActive(true);
    SetPaused(true);
    SetupOption();
}
public void HideMenu(){
    menu.SetActive(false);
    SetPaused(false);
}
void SetPaused(bool value){
    if(value){
        Cursor.lockState=CursorLockMode.None;
        Time.timeScale=0f;
    }else{
        Cursor.lockState=CursorLockMode.Locked;
        Time.timeScale=1f;
    }
}
public void SelectCard(int slotIdx)
{
    if (cards[slotIdx] != null)
    {
        // Obtener el buff de la carta seleccionada
        Buff cardBuff = cards[slotIdx].GetBuff();
        
        if (cardBuff != null)
        {
            // Buscar al jugador y aplicar el buff
            PlayerController player = FindAnyObjectByType<PlayerController>(); // CAMBIADO
            if (player != null)
            {
                BuffManager buffManager = player.GetComponent<BuffManager>();
                if (buffManager != null)
                {
                    buffManager.ApplyBuff(cardBuff);
                    
                    // Mostrar mensaje de buff aplicado
                    UIMsg uiMsg = FindAnyObjectByType<UIMsg>(); // CAMBIADO
                    if (uiMsg != null)
                    {
                        uiMsg.Say($"¡{cardBuff.buffName} aplicado!");
                    }
                }
            }
        }
        
        // Ejecutar la acción específica de la carta
        cards[slotIdx].Action();
    }
    
    HideMenu();
}


}