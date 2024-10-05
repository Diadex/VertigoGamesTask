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
                    // Get the current (grayscale) color
                    Color grayscaleColor = imageColorComponent.color;

                    // Convert the current color to grayscale value (luminance)
                    float grayscaleValue = grayscaleColor.r * 0.299f + grayscaleColor.g * 0.587f + grayscaleColor.b * 0.114f;

                    // Get the new color from the slot item
                    Color newColor = slotItem.GetColor();

                    // Blend the new color with the grayscale intensity
                    Color blendedColor = new Color(
                        Mathf.Lerp(grayscaleValue, newColor.r, colorMixAmount[i]),
                        Mathf.Lerp(grayscaleValue, newColor.g, colorMixAmount[i]),
                        Mathf.Lerp(grayscaleValue, newColor.b, colorMixAmount[i])
                    );

                    // Retain the alpha of the current color
                    imageColorComponent.color = new Color(blendedColor.r, blendedColor.g, blendedColor.b, grayscaleColor.a);
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
