using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using StandaloneItems;

public class SlotUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ImageGameObject; // Reference to the image GameObject
    [SerializeField]
    private GameObject TextGameObject; // Reference to the amount writing GameObject

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
            textComponent.text = GetTextUIAmountDisplay(slotItem.GetAmount());
        }
    }

    private string GetTextUIAmountDisplay(int amount)
    {
        if (amount <= 0)
        {
            return "";
        }
        else if (amount % 1000000 == 0)
        {
            amount = amount / 1000000;
            return "x" + amount + "M";
        }
        else if (amount % 1000 == 0)
        {
            amount = amount / 1000;
            return "x" + amount + "K";
        }
        return "x" + amount;
    }

}
