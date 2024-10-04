using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StandaloneItems;

// The Spinner result's UI manager
public class SpinnerResultManager : MonoBehaviour
{
    [SerializeField]
    private ItemDisplayUIManager obtainedCardUI;
    [SerializeField]
    private TextMeshProUGUI obtainedItemNameText;
    [SerializeField]
    private TextMeshProUGUI explosionMoneyAmountText;
    [SerializeField]
    private GameObject explodeResultGameObject;
    [SerializeField]
    private GameObject wonResultGameObject;

    [SerializeField]
    private int costOnExplosion = 10000;

    private AmountDisplayFormatter amountDisplayFormatter = new AmountDisplayFormatter();
    // returns true if exploded
    public bool DisplaySpinnerResultUI(Obtainable obtainedResult)
    {
        if (obtainedResult.GetAmount() < 0)
        {
            SetUICostOnExplosion();
            DisplayExplodeResultUI();
            return true;
        }
        else
        {
            obtainedCardUI.SetImageAndText(obtainedResult);
            obtainedItemNameText.text = "You Won " + obtainedResult.GetName() + "!";
            DisplayWonItemResultUI();
            return false;
        }
    }

    private void SetUICostOnExplosion()
    {
        explosionMoneyAmountText.text = amountDisplayFormatter.GetTextUIAmountDisplay(costOnExplosion, true);
    }

    public int GetCostOnExplosion()
    {
        return costOnExplosion;
    }

    private void DisplayExplodeResultUI()
    {
        explodeResultGameObject.SetActive(true);
    }

    private void DisplayWonItemResultUI()
    {
        wonResultGameObject.SetActive(true);
    }

    public void HideExplodeResultUI()
    {
        explodeResultGameObject.SetActive(false);
    }

    public void HideWonResultUI()
    {
        wonResultGameObject.SetActive(false);
    }

}
