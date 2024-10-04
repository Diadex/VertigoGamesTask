using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDisplayInformationManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI cashCurrencyText;
    [SerializeField]
    private TextMeshProUGUI goldCurrencyText;
    [SerializeField]
    private TextMeshProUGUI roundText;

    public void SetCashCurrencyText(string cash)
    {
        cashCurrencyText.text = cash;
    }
    public void SetGoldCurrencyText(string gold)
    {
        goldCurrencyText.text = gold;
    }
    public void SetRoundText(int round)
    {
        roundText.text = "Round " + round;
    }
}
