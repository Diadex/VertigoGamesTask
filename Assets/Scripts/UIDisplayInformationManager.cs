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
    [SerializeField]
    private string cashName = "cash";
    [SerializeField]
    private string goldName = "gold";

    private AmountDisplayFormatter formatter = new AmountDisplayFormatter();

    public void DisplayCurrencyInfo( List<(string currency, int amount)> currencies)
    {
        int currencyCount = currencies.Count;
        for (int i = 0; i < currencyCount; i ++)
        {
            string currentCurrency = currencies[i].currency;
            if (currentCurrency.Equals(cashName))
            {
                SetCashCurrencyText(currencies[i].amount);
            }
            else if (currentCurrency.Equals(goldName))
            {
                SetGoldCurrencyText(currencies[i].amount);
            }
        }
    }

    private void SetCashCurrencyText(int cash)
    {
        cashCurrencyText.text = formatter.GetTextUIAmountDisplay(cash, true);
    }
    private void SetGoldCurrencyText(int gold)
    {
        goldCurrencyText.text = formatter.GetTextUIAmountDisplay(gold, true);
    }
    public void SetRoundText(int round)
    {
        roundText.text = "Round " + round;
    }
}
