using StandaloneItems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Coordinates the spinner flow
public class SpinnerManager : MonoBehaviour
{
    [SerializeField]
    private SpinnerContentManager spinnerContentManager;
    [SerializeField]
    private SpinnerSlotPlacer spinnerSlotPlacer;
    [SerializeField]
    private SpinnerWheelUIManager spinnerWheelUIManager;
    [SerializeField]
    private SpinnerAnimator spinnerAnimator;
    [SerializeField]
    private SpinnerResultManager spinnerResultManager;
    [SerializeField]
    private UIDisplayInformationManager uiDisplayInformationManager;
    [SerializeField]
    private SpinnerPanelUIManager spinnerPanelUIManager;
    [SerializeField]
    private ObtainedItemsManager obtainedItemsManager;
    [SerializeField]
    private ObtainedItemsUIManager obtainedItemsUIManager;
    [SerializeField]
    private ChestOpenManager chestOpenManager;
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

    private enum SpinnerState
    {
        Initializing,
        WaitingForButtonPress,
        Spinning,
        ResultDisplayed,
        Exploded,
        SetRoundToBeginning,
        ObtainableItemsDisplayed,
        ChestsOpening,
        ChestOpen
    }

    private SpinnerState currentState;

    private int round = 0;
    private int spinnerResult = 0;
    private int numberOfItems;
    private Obtainable itemResult;
    private GameObject[] spinnerWheels;
    private List<Obtainable> openedChestContents;

    private void Start()
    {
        spinnerWheels = spinnerWheelUIManager.GetSpinnerWheelUIReferences();
        currentState = SpinnerState.SetRoundToBeginning;
        obtainedItemsUIManager.SetUIObtainedStorage();
    }

    private void Update()
    {
        switch (currentState)
        {
            case SpinnerState.Initializing:
                InitializeSpinner();
                currentState = SpinnerState.WaitingForButtonPress;
                break;

            case SpinnerState.WaitingForButtonPress:
                if (spinnerButtonHandler.GetFlag())
                {
                    spinnerButtonHandler.SetFlag(false);
                    spinnerButtonHandler.ButtonSetActive(false);
                    CheckEnableSafeZoneOptions(false);

                    spinnerAnimator.SpinWheels(spinnerWheels, spinnerResult, numberOfItems);
                    currentState = SpinnerState.Spinning;
                }
                else if (leaveButtonHandler.GetFlag())
                {
                    leaveButtonHandler.SetFlag(false);
                    leaveButtonHandler.ButtonSetActive(false);
                    CheckEnableSafeZoneOptions(false);
                    // save teh chests in temporary to the chestOpenManager
                    chestOpenManager.SaveChestsToOpenManager();
                    if (chestOpenManager.GetThereIsChestInTemp())
                    {
                        openedChestContents = chestOpenManager.OpenChest();
                        chestOpenUIManager.OpenChestUIActive(true);
                        chestOpenButton.ButtonSetActive(true);
                        currentState = SpinnerState.ChestsOpening;
                    }
                    else
                    {
                        obtainedItemsManager.MoveTemporaryToPermanentStorage();
                        chestOpenButton.ButtonSetActive(false);
                        currentState = SpinnerState.SetRoundToBeginning;
                    }
                }
                else if (obtainedItemsUIDisplayButton.GetFlag())
                {
                    spinnerButtonHandler.ButtonSetActive(false);
                    obtainedItemsUIDisplayButton.SetFlag(false);
                    // open the UI for obtained items
                    obtainedItemsUIManager.SetUIObtainedStorage();
                    obtainedItemsUIManager.EnableUI(true);
                    currentState = SpinnerState.ObtainableItemsDisplayed;
                }

                if (currentState != SpinnerState.WaitingForButtonPress)
                {
                    // display the obtainedItemsUIButton unless we are at this state
                    obtainedItemsUIDisplayButton.ButtonSetActive(false);
                }
                break;

            case SpinnerState.ObtainableItemsDisplayed:
                if (obtainedItemsUIDisplayExitButton.GetFlag())
                {
                    spinnerButtonHandler.ButtonSetActive(true);
                    obtainedItemsUIDisplayExitButton.SetFlag(false);
                    obtainedItemsUIManager.EnableUI(false);
                    obtainedItemsUIDisplayButton.ButtonSetActive(true);
                    currentState = SpinnerState.WaitingForButtonPress;
                }
                break;

            case SpinnerState.Spinning:
                if (!spinnerAnimator.CheckAnimation())
                {
                    obtainedItemsManager.AddItemToStorage(ObtainedItemsManager.StorageType.TempStorage, itemResult);
                    // check if we exploded or not and display the UI accordingly.
                    if (spinnerResultManager.DisplaySpinnerResultUI(itemResult))
                    {
                        currentState = SpinnerState.Exploded;
                    }
                    else
                    {
                        currentState = SpinnerState.ResultDisplayed;
                    }
                }
                break;

            case SpinnerState.ResultDisplayed:
                if (wonResultButtonHandler.GetFlag())
                {
                    spinnerResultManager.HideWonResultUI();
                    wonResultButtonHandler.SetFlag(false);
                    currentState = SpinnerState.Initializing;
                }
                break;

            case SpinnerState.Exploded:
                if (explodeResultGiveUpButtonHandler.GetFlag())
                {
                    spinnerResultManager.HideExplodeResultUI();
                    explodeResultGiveUpButtonHandler.SetFlag(false);

                    obtainedItemsManager.ClearTempStorage();
                    currentState = SpinnerState.SetRoundToBeginning;
                }
                else if (explodeResultReviveButtonHandler.GetFlag())
                {
                    explodeResultReviveButtonHandler.SetFlag(false);
                    int reviveCostGold = spinnerResultManager.GetCostOnExplosion();
                    // spend gold, update the gold UI, then continue from the next round
                    if (obtainedItemsManager.SpendGold(reviveCostGold))
                    {
                        uiDisplayInformationManager.DisplayCurrencyInfo(obtainedItemsManager.GetCurrencyList());
                        spinnerResultManager.HideExplodeResultUI();
                        currentState = SpinnerState.Initializing;
                    }
                }
                break;
            case SpinnerState.SetRoundToBeginning:
                round = 0;
                uiDisplayInformationManager.DisplayCurrencyInfo(obtainedItemsManager.GetCurrencyList());
                currentState = SpinnerState.Initializing;
                break;
            case SpinnerState.ChestsOpening:
                if (chestOpenButton.GetFlag())
                {
                    chestOpenButton.SetFlag(false);
                    chestOpenButton.ButtonSetActive(false);
                    wonResultButtonHandler.SetFlag(true);
                    currentState = SpinnerState.ChestOpen;
                }

                break;
            case SpinnerState.ChestOpen:

                if (wonResultButtonHandler.GetFlag())
                {
                    if (openedChestContents.Count > 0)
                    {
                        wonResultButtonHandler.SetFlag(false);
                        spinnerResultManager.RestartAnimation();
                        spinnerResultManager.DisplaySpinnerResultUI(openedChestContents[0]);
                        // remove from the openedChestContents
                        openedChestContents.RemoveAt(0);
                        currentState = SpinnerState.ChestOpen;
                    }
                    else
                    {
                        if (chestOpenManager.GetThereIsChestInTemp())
                        {
                            spinnerResultManager.HideWonResultUI();
                            wonResultButtonHandler.SetFlag(false);
                            openedChestContents = chestOpenManager.OpenChest();
                            chestOpenButton.ButtonSetActive(true);
                            currentState = SpinnerState.ChestsOpening;
                        }
                        else {
                            obtainedItemsManager.MoveTemporaryToPermanentStorage();
                            chestOpenButton.ButtonSetActive(false);
                            chestOpenUIManager.OpenChestUIActive(false);
                            spinnerResultManager.HideWonResultUI();
                            wonResultButtonHandler.SetFlag(false);
                            currentState = SpinnerState.SetRoundToBeginning;
                        }
                    }
                }
                break;

        }
    }

    private void InitializeSpinner()
    {
        round++;
        uiDisplayInformationManager.SetRoundText(round);
        string roundType = spinnerWheelUIManager.SpinnerInitialise(round);

        List<Obtainable> spinnerObtainables = spinnerContentManager.GetSpinnerContents(roundType);
        bool isSafeZone = spinnerContentManager.GetCurrentSpinnerIsLeavable();
        CheckEnableSafeZoneOptions(isSafeZone);
        spinnerSlotPlacer.SetSlotObtainableItems(spinnerObtainables);

        numberOfItems = spinnerObtainables.Count;
        spinnerResult = UnityEngine.Random.Range(0, numberOfItems);
        itemResult = spinnerObtainables[spinnerResult];
        
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
