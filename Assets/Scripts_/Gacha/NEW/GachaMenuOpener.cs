using UnityEngine;

public class GachaMenuOpener : MonoBehaviour
{
    public CardUpgradeGachaMenuSO gachaMenuSO; // Asigna el script en el inspector

    // Llama este método desde un botón, evento, etc.
    public void OpenGachaMenu()
    {
        gachaMenuSO.menu.SetActive(true); // Muestra el panel
        gachaMenuSO.SetupOption();        // Asigna las cartas y datos a los slots
        // Si tienes lógica de pausa, puedes llamarla aquí también
    }
}