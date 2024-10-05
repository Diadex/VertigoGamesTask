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
    [SerializeField]
    private GameObject buttonsPanel;
    [SerializeField]
    private GameObject leaveButton;

    public void SetSpinWriting(string spinType, string spinRewardWriting, Color color)
    {
        spinTypeWriting.text = "" + spinType.ToUpper() + " SPIN!";
        spinTypeWriting.color = color;
        spinRewardRateWriting.text = spinRewardWriting;
        spinRewardRateWriting.color = color;
    }

    public void OpenButtonsPanel( bool isOpen)
    {
        buttonsPanel.SetActive(isOpen);
    }

    public void SetActiveLeaveButton( bool isOpen)
    {
        leaveButton.SetActive(isOpen);
    }


}
