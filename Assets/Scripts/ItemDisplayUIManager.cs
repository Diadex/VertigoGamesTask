using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using StandaloneItems;

public class ItemDisplayUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ImageGameObject; // Reference to the image GameObject
    [SerializeField]
    private GameObject TextGameObject; // Reference to the amount writing GameObject

    private AmountDisplayFormatter formatter = new AmountDisplayFormatter();

    public void SetImageAndText(Obtainable slotItem)
    {
        Image imageComponent = ImageGameObject.GetComponent<Image>();
        if (imageComponent != null)
        {
            imageComponent.sprite = slotItem.GetImage(); // Assuming Obtainable has an 'image' property of type Sprite
            imageComponent.color = slotItem.GetColor(); // Assuming Obtainable has a 'color' property of type Color
        }

        TextMeshProUGUI textComponent = TextGameObject.GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = formatter.GetTextUIAmountDisplayTimes(slotItem.GetAmount(), false);
        }
    }


}
