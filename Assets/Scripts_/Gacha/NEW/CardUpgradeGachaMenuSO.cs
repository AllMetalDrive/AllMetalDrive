/*******************************************************
* Project: All Metal Drive
* Script: CardUpgradeGachaMenuSO.cs
* Author: Adaptado por GitHub Copilot
* Created: 30/11/2025
*
* Description:
* GachaMenu adaptado para usar CardUpgradeSO ScriptableObjects.
* Permite mostrar y aplicar upgrades de cartas usando ScriptableObjects.
* El código original basado en Card.cs está comentado para referencia.
*******************************************************/

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUpgradeGachaMenuSO : MonoBehaviour
{
    [Header("CARD UPGRADES")]
    public CardUpgradeSO[] prizes5s;
    public CardUpgradeSO[] prizes4s;
    public CardUpgradeSO[] prizes3s;

    [Header("PROBABILITIES")]
    public float probability5s = 0.01f;
    public float probability4s = 1.0f;

    [Header("PITY")]
    public int pity5s = 90;
    public int pity4s = 10;
    int count5s = 1;
    int count4s = 1;

    [Header("OPTIONS")]
    public bool hasPity = false;
    public bool alsoReset4s = true;
    public bool sharedRarity = false;
    public bool allowDupes = false;

    [Header("COMPONENTS")]
    public GameObject menu;
    public GameObject[] cardSlots; // Cada slot debe tener Image, TMP_Text para nombre y descripción

    [Header("CARD ASSIGNMENT")]
    int[] rarities;
    int[] prizeIdx;
    CardUpgradeSO[] upgrades;

    void Start()
    {
        if (cardSlots.Length == 0 || menu == null)
        {
            Debug.Log("Warning: CardUpgradeGachaMenuSO: Please assign all components and card slots!");
        }
        else
        {
            rarities = new int[cardSlots.Length];
            prizeIdx = new int[cardSlots.Length];
            upgrades = new CardUpgradeSO[cardSlots.Length];
        }
    }

    public void SetupOption()
    {
        if (sharedRarity)
        {
            int rarity = CalculateRarity();
            for (int i = 0; i < rarities.Length; i++)
            {
                rarities[i] = rarity;
            }
        }
        else
        {
            for (int i = 0; i < rarities.Length; i++)
            {
                rarities[i] = CalculateRarity();
            }
        }
        if (!allowDupes)
        {
            bool isValid = false;
            int maxTries = 100;
            int tries = 0;
            while (!isValid && tries < maxTries)
            {
                GenerateIndex();
                isValid = true;
                int i = 0;
                int j = 0;
                while (i < prizeIdx.Length)
                {
                    j = i + 1;
                    while (j < prizeIdx.Length)
                    {
                        if (prizeIdx[i] == prizeIdx[j] && rarities[i] == rarities[j])
                        {
                            isValid = false;
                        }
                        j++;
                    }
                    i++;
                }
                tries++;
            }
            if (!isValid)
            {
                Debug.LogError("No se pudo generar una combinación válida sin duplicados después de 100 intentos.");
            }
        }
        else
        {
            GenerateIndex();
        }
        for (int i = 0; i < prizeIdx.Length; i++)
        {
            CardUpgradeSO upgrade;
            switch (rarities[i])
            {
                case 5:
                    upgrade = prizes5s[prizeIdx[i]];
                    break;
                case 4:
                    upgrade = prizes4s[prizeIdx[i]];
                    break;
                default:
                    upgrade = prizes3s[prizeIdx[i]];
                    break;
            }
            upgrades[i] = upgrade;

            // Usa el script CardSlotUI para asignar los datos
            var slotUI = cardSlots[i].GetComponent<CardSlotUI>();
            if (slotUI != null)
            {
                slotUI.SetCard(upgrade);
            }
            else
            {
                Debug.LogWarning($"CardSlotUI no encontrado en el slot {i}");
            }
        }
    }

    void GenerateIndex()
    {
        for (int i = 0; i < prizeIdx.Length; i++)
        {
            switch (rarities[i])
            {
                case 5:
                    prizeIdx[i] = Random.Range(0, prizes5s.Length);
                    break;
                case 4:
                    prizeIdx[i] = Random.Range(0, prizes4s.Length);
                    break;
                default:
                    prizeIdx[i] = Random.Range(0, prizes3s.Length);
                    break;
            }
        }
    }

    int CalculateRarity()
    {
        int rarity;
        if (hasPity && count5s >= pity5s)
        {
            rarity = 5;
            CountPity(5);
        }
        else if (hasPity && count4s >= pity4s)
        {
            rarity = 4;
            CountPity(4);
        }
        else
        {
            float probability = Random.Range(0.001f, 99.999f);
            if (probability <= probability5s)
            {
                rarity = 5;
                CountPity(5);
            }
            else if (probability <= probability4s + probability5s)
            {
                rarity = 4;
                CountPity(4);
            }
            else
            {
                rarity = 3;
                CountPity(3);
            }
        }
        return rarity;
    }

    void CountPity(int wonRarity)
    {
        switch (wonRarity)
        {
            case 5:
                count5s = 1;
                if (alsoReset4s)
                {
                    count4s = 1;
                }
                else
                {
                    count4s++;
                }
                break;
            case 4:
                count4s = 1;
                count5s++;
                break;
            default:
                count5s++;
                count4s++;
                break;
        }
    }


    public void ShowMenu()
    {
        menu.SetActive(true);
        //SetPaused(true);
        SetupOption();
    }
    public void HideMenu()
    {
        menu.SetActive(false);
        //SetPaused(false);
    }


    /*  void SetPaused(bool value)
     {
         if (value)
         {
             Cursor.lockState = CursorLockMode.None;
             Time.timeScale = 0f;
         }
         else
         {
             Cursor.lockState = CursorLockMode.Locked;
             Time.timeScale = 1f;
         }
     } */

    public void SelectCard(int slotIdx)
    {
        upgrades[slotIdx].ApplyUpgrade();
        //HideMenu();
    }



    /*
    // --- CÓDIGO ORIGINAL BASADO EN Card.cs ---
    
    public Card[] prizes5s;
    public Card[] prizes4s;
    public Card[] prizes3s;
    Card[] cards;

    public void SelectCard(int slotIdx)
    {
        cards[slotIdx].Action();
        HideMenu();
    }
    */
}

