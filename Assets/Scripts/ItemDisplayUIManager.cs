using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using StandaloneItems;

public class ItemDisplayUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject imageGameObject; // Reference to the image GameObject
    [SerializeField]
    private List<GameObject> colorComponent; // Reference to the image GameObject
    [SerializeField]
    private GameObject textGameObject; // Reference to the amount writing GameObject
    [SerializeField]
    private List<float> colorMixAmount;

    private AmountDisplayFormatter formatter = new AmountDisplayFormatter();

    public void SetImageAndText(Obtainable slotItem)
    {
        if (imageGameObject)
        {
            Image imageComponent = imageGameObject.GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = slotItem.GetImage(); // Assuming Obtainable has an 'image' property of type Sprite
            }
        }

        if (colorComponent != null && colorComponent.Count > 0)
        {
            for (int i = 0; i < colorComponent.Count; i++)
            {
                Image imageColorComponent = colorComponent[i].GetComponent<Image>();
                if (imageColorComponent != null)
                {
                    // Get the current color
                    Color currentColor = imageColorComponent.color;

                    // Get the new color from the slot item
                    Color newColor = slotItem.GetColor();

                    // Interpolate between the current color and the new color

                    // Create the blended color
                    Color blendedColor = Color.Lerp(currentColor, newColor, colorMixAmount[i]);

                    // Set the alpha to the current alpha value
                    imageColorComponent.color = new Color(blendedColor.r, blendedColor.g, blendedColor.b, currentColor.a);
                }

            }
        }

        if (textGameObject)
        {
            TextMeshProUGUI textComponent = textGameObject.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = formatter.GetTextUIAmountDisplayTimes(slotItem.GetAmount(), false);
            }
        }
    }


}
