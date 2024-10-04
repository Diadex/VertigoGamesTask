using System;

public class AmountDisplayFormatter
{
    // Method to format the amount for UI display
    public string GetTextUIAmountDisplayTimes(int amount, bool zeroAllowed)
    {
        if (amount == 0 && zeroAllowed)
        {
            return "x0";
        }
        else if (amount < 0)
        {
            return "";
        }
        else if (amount > 1000000)
        {
            amount = amount / 1000000;
            return "x" + amount + "M";
        }
        else if (amount > 1000)
        {
            amount = amount / 1000;
            return "x" + amount + "K";
        }
        return "x" + amount;
    }
    public string GetTextUIAmountDisplay(int amount, bool zeroAllowed)
    {
        if (amount == 0 && zeroAllowed)
        {
            return "0";
        }
        else if (amount < 0)
        {
            return "";
        }
        else if (amount > 10000000)
        {
            amount = amount / 1000000;
            return "" + amount + "M";
        }
        else if (amount > 10000)
        {
            amount = amount / 1000;
            return "" + amount + "K";
        }
        return "" + amount;
    }
}
