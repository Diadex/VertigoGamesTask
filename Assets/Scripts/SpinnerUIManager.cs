using StandaloneItems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerUIManager : MonoBehaviour
{
    [SerializeField]
    private SpinnerContentManager spinnerContentManager;
    [SerializeField]
    private SpinnerSlotPlacer spinnerSlotPlacer;
    [SerializeField]
    private SpinnerWheelUIManager spinnerWheelUIManager;
    [SerializeField]
    private SpinnerResultManager spinnerResultManager;
    [SerializeField]
    private UIDisplayInformationManager uiDisplayInformationManager;
    [SerializeField]
    private SpinnerPanelUIManager spinnerPanelUIManager;
    [SerializeField]
    private ObtainedItemsUIManager obtainedItemsUIManager;
    [SerializeField]
    private ChestOpenUIManager chestOpenUIManager;

    [SerializeField]
    private ButtonHandler spinnerButtonHandler;
    [SerializeField]
    private ButtonHandler leaveButtonHandler;
    [SerializeField]
    private ButtonHandler explodeResultGiveUpButtonHandler;
    [SerializeField]
    private ButtonHandler explodeResultReviveButtonHandler;
    [SerializeField]
    private ButtonHandler wonResultButtonHandler;
    [SerializeField]
    private ButtonHandler obtainedItemsUIDisplayButton;
    [SerializeField]
    private ButtonHandler obtainedItemsUIDisplayExitButton;
    [SerializeField]
    private ButtonHandler chestOpenButton;


    private void Start()
    {
        obtainedItemsUIManager.SetUIObtainedStorage();
    }


    public bool GetIsSafeZone()
    {
        return spinnerContentManager.GetCurrentSpinnerIsLeavable();
    }
    public string InitSpinnerGetRoundType(int round)
    {
        return spinnerWheelUIManager.SpinnerInitialise(round);
    }
    public List<Obtainable> GetSpinnerContents(string round)
    {
        return spinnerContentManager.GetSpinnerContents(round);
    }

    public GameObject[] GetSpinnerWheelsReferences()
    {
        return spinnerWheelUIManager.GetSpinnerWheelUIReferences();
    }

    public void SpinnerButtonClicked()
    {
        spinnerButtonHandler.SetFlag(false);
        spinnerButtonHandler.ButtonSetActive(false);
        CheckEnableSafeZoneOptions(false);
    }

    public void LeaveButtonPressed()
    {
        leaveButtonHandler.SetFlag(false);
        leaveButtonHandler.ButtonSetActive(false);
        CheckEnableSafeZoneOptions(false);
    }

    public void BeginChestOpening()
    {
        chestOpenUIManager.OpenChestUIActive(true);
        chestOpenButton.ButtonSetActive(true);
    }

    public void OpenObtainedItems()
    {
        spinnerButtonHandler.ButtonSetActive(false);
        obtainedItemsUIDisplayButton.SetFlag(false);
        // open the UI for obtained items
        obtainedItemsUIManager.SetUIObtainedStorage();
        obtainedItemsUIManager.EnableUI(true);
    }


    public void ObtainableItemsButtonSetDisabled()
    {
        obtainedItemsUIDisplayButton.ButtonSetActive(false);
    }

    public void ObtainedItemsExited()
    {
        spinnerButtonHandler.ButtonSetActive(true);
        obtainedItemsUIDisplayExitButton.SetFlag(false);
        obtainedItemsUIManager.EnableUI(false);
        obtainedItemsUIDisplayButton.ButtonSetActive(true);
    }


    public void ExitWonResult()
    {
        spinnerResultManager.HideWonResultUI();
        wonResultButtonHandler.SetFlag(false);
    }
    public void ReEnableGiveUpResultButton()
    {
        explodeResultGiveUpButtonHandler.SetFlag(false);
    }
    public void HideExplodeUI()
    {
        spinnerResultManager.HideExplodeResultUI();
    }

    public void ReEnableExplodeResultButton()
    {
        explodeResultReviveButtonHandler.SetFlag(false);
    }

    public void DisplayCurrencyInfo( List<(string currency, int info)> currencyList)
    {
        uiDisplayInformationManager.DisplayCurrencyInfo(currencyList);
    }

    public void ChestOpenDeactivate()
    {
        ChestOpenButtonDisable();
        chestOpenButton.ButtonSetActive(false);
        wonResultButtonHandler.SetFlag(true);
    }
    public void ChestOpenButtonDisable()
    {
        chestOpenButton.SetFlag(false);
    }

    public bool GetIsSpinClicked()
    {
        return spinnerButtonHandler.GetFlag();
    }
    public bool GetIsLeaveClicked()
    {
        return leaveButtonHandler.GetFlag();
    }
    public bool GetIsExplodeAndDisplayUI(Obtainable itemResult)
    {
        return spinnerResultManager.DisplaySpinnerResultUI(itemResult);
    }
    public bool GetIsObtainedItemsOpenClicked()
    {
        return obtainedItemsUIDisplayButton.GetFlag();
    }
    public bool GetIsObtainedItemsExitClicked()
    {
        return obtainedItemsUIDisplayExitButton.GetFlag();
    }
    public bool GetIsExplodeGiveUpClicked()
    {
        return explodeResultGiveUpButtonHandler.GetFlag();
    }
    public bool GetIsExplodeReviveClicked()
    {
        return explodeResultReviveButtonHandler.GetFlag();
    }
    public bool GetIsChestOpenClicked()
    {
        return chestOpenButton.GetFlag();
    }
    public bool GetIsCloseResultViewClicked()
    {
        return wonResultButtonHandler.GetFlag();
    }


    public void ContinueWithNextItem( Obtainable item)
    {
        wonResultButtonHandler.SetFlag(false);
        spinnerResultManager.RestartAnimation();
        spinnerResultManager.DisplaySpinnerResultUI(item);
    }

    public void ContinueChestOpeningNextChest()
    {
        spinnerResultManager.HideWonResultUI();
        wonResultButtonHandler.SetFlag(false);
        chestOpenButton.ButtonSetActive(true);
    }

    public void EndChestOpeningAndReset() 
    {
        chestOpenButton.ButtonSetActive(false);
        chestOpenUIManager.OpenChestUIActive(false);
        spinnerResultManager.HideWonResultUI();
        wonResultButtonHandler.SetFlag(false);
    }

    public void InitializeSpinner(int round, bool isSafeZone, List<Obtainable> spinnerObtainables, string roundType)
    {
        uiDisplayInformationManager.SetRoundText(round);
        CheckEnableSafeZoneOptions(isSafeZone);
        spinnerSlotPlacer.SetSlotObtainableItems(spinnerObtainables);
        spinnerPanelUIManager.SetSpinWriting(roundType,
                            spinnerContentManager.GetObtainableRewardRateWriting(), spinnerContentManager.GetSpinnerZoneColor());
        spinnerButtonHandler.ButtonSetActive(true);
        leaveButtonHandler.ButtonSetActive(true);
        obtainedItemsUIDisplayButton.ButtonSetActive(true);
    }

    private void CheckEnableSafeZoneOptions(bool isSafeZone)
    {
        if (isSafeZone)
        {
            spinnerPanelUIManager.OpenButtonsPanel(true);
            spinnerPanelUIManager.SetActiveLeaveButton(true);
        }
        else
        {
            spinnerPanelUIManager.OpenButtonsPanel(false);
            spinnerPanelUIManager.SetActiveLeaveButton(false);
        }
    }

}
