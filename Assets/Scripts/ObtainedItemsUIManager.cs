using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainedItemsUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject uiElementObtainedItemsDisplay;

    public void enableUI( bool isEnabled)
    {
        uiElementObtainedItemsDisplay.SetActive(isEnabled);
    }

}
