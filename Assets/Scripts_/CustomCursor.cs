using UnityEngine;
using UnityEngine.UI;

public class UICursor : MonoBehaviour
{
    public Image cursorImage;          // La imagen UI que representa la mira
    public Sprite defaultSprite;       // Sprite inicial de la mira

    void Start()
    {
        // Ocultar el puntero real
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        // Asignar sprite por defecto
        cursorImage.sprite = defaultSprite;
    }

    void Update()
    {
        // Posicionar la imagen UI en la posición del mouse
        cursorImage.rectTransform.position = Input.mousePosition;
    }

    // Cambiar sprite de la mira en cualquier momento
    public void SetCursorSprite(Sprite newSprite)
    {
        cursorImage.sprite = newSprite;
    }
}
