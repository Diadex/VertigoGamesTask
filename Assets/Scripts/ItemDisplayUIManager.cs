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
                    imageColorComponent.color = slotItem.GetColor(); // Assuming Obtainable has a 'color' property of type Color
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
