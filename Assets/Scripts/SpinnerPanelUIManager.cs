using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpinnerPanelUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI spinTypeWriting;

    [SerializeField]
    private TextMeshProUGUI spinRewardRateWriting;


    public void SetSpinWriting(string spinType, string spinRewardWriting)
    {
        spinTypeWriting.text = "" + spinType.ToUpper() + " SPIN!";
        spinRewardRateWriting.text = spinRewardWriting;
    }

    // TODO make the leave button pop up


}
