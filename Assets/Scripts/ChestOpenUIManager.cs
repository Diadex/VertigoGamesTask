using StandaloneItems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestOpenUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject chestOpenUI;
    [SerializeField]
    private ItemDisplayUIManager chestImageDisplay;
    [SerializeField]
    private TextMeshProUGUI chestNameDisplay;


    public void UpdateChestOpenUI(Obtainable chestItem)
    {
        chestImageDisplay.SetImageAndText(chestItem);
        chestNameDisplay.text = chestItem.GetName();
    }

    public void OpenChestUIActive( bool isActive)
    {
        chestOpenUI.SetActive(isActive);
    }


}
