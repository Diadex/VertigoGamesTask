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
        obtainedItemsUIManager.SetUIObtainedStorage();
        currentState = SpinnerState.SetRoundToBeginning;
    }

    private void Update()
    {
        switch (currentState)
        {
            case SpinnerState.Initializing:
                // Functiion
                InitializeSpinner();
                // State
                currentState = SpinnerState.WaitingForButtonPress;
                break;
            case SpinnerState.WaitingForButtonPress:
                // UI
                if (spinnerButtonHandler.GetFlag())
                {
                    // UI
                    spinnerButtonHandler.SetFlag(false);
                    spinnerButtonHandler.ButtonSetActive(false);
                    CheckEnableSafeZoneOptions(false);
                    // Function
                    spinnerAnimator.SpinWheels(spinnerWheels, spinnerResult, numberOfItems);
                    // State
                    currentState = SpinnerState.Spinning;
                }// UI
                else if (leaveButtonHandler.GetFlag())
                {
                    // UI
                    leaveButtonHandler.SetFlag(false);
                    leaveButtonHandler.ButtonSetActive(false);
                    CheckEnableSafeZoneOptions(false);
                    // Function (save)
                    chestOpenManager.SaveChestsToOpenManager();
                    // Function (chests)
                    if (chestOpenManager.GetThereIsChestInTemp())
                    {
                        // UI
                        chestOpenUIManager.OpenChestUIActive(true);
                        chestOpenButton.ButtonSetActive(true);
                        // Function
                        openedChestContents = chestOpenManager.OpenChest();
                        // State
                        currentState = SpinnerState.ChestsOpening;
                    }
                    else
                    {
                        // UI
                        chestOpenButton.ButtonSetActive(false);
                        // Function
                        obtainedItemsManager.MoveTemporaryToPermanentStorage();
                        // State
                        currentState = SpinnerState.SetRoundToBeginning;
                    }
                }// UI
                else if (obtainedItemsUIDisplayButton.GetFlag())
                {
                    spinnerButtonHandler.ButtonSetActive(false);
                    obtainedItemsUIDisplayButton.SetFlag(false);
                    // open the UI for obtained items
                    obtainedItemsUIManager.SetUIObtainedStorage();
                    obtainedItemsUIManager.EnableUI(true);
                    currentState = SpinnerState.ObtainableItemsDisplayed;
                }
                // State
                if (currentState != SpinnerState.WaitingForButtonPress)
                {
                    //UI
                    // display the obtainedItemsUIButton unless we are at this state
                    obtainedItemsUIDisplayButton.ButtonSetActive(false);
                }
                break;
            case SpinnerState.ObtainableItemsDisplayed:
                // UI
                if (obtainedItemsUIDisplayExitButton.GetFlag())
                {
                    // UI
                    spinnerButtonHandler.ButtonSetActive(true);
                    obtainedItemsUIDisplayExitButton.SetFlag(false);
                    obtainedItemsUIManager.EnableUI(false);
                    obtainedItemsUIDisplayButton.ButtonSetActive(true);
                    // State
                    currentState = SpinnerState.WaitingForButtonPress;
                }
                break;

            case SpinnerState.Spinning:
                // Function
                if (!spinnerAnimator.CheckAnimation())
                {
                    // Function
                    obtainedItemsManager.AddItemToStorage(ObtainedItemsManager.StorageType.TempStorage, itemResult);
                    // check if we exploded or not and display the UI accordingly.
                    // UI
                    if (spinnerResultManager.DisplaySpinnerResultUI(itemResult))
                    {
                        // State
                        currentState = SpinnerState.Exploded;
                    }
                    else
                    {
                        // State
                        currentState = SpinnerState.ResultDisplayed;
                    }
                }
                break;
            case SpinnerState.ResultDisplayed:
                // UI
                if (wonResultButtonHandler.GetFlag())
                {
                    // UI
                    spinnerResultManager.HideWonResultUI();
                    wonResultButtonHandler.SetFlag(false);
                    // State
                    currentState = SpinnerState.Initializing;
                }
                break;
            case SpinnerState.Exploded:
                // UI
                if (explodeResultGiveUpButtonHandler.GetFlag())
                {
                    // UI
                    spinnerResultManager.HideExplodeResultUI();
                    explodeResultGiveUpButtonHandler.SetFlag(false);
                    // Function (save)
                    obtainedItemsManager.ClearTempStorage();
                    // State
                    currentState = SpinnerState.SetRoundToBeginning;
                }// UI
                else if (explodeResultReviveButtonHandler.GetFlag())
                {
                    // UI
                    explodeResultReviveButtonHandler.SetFlag(false);
                    // Function
                    int reviveCostGold = spinnerResultManager.GetCostOnExplosion();
                    // spend gold, update the gold UI, then continue from the next round
                    if (obtainedItemsManager.SpendGold(reviveCostGold))
                    {
                        // UI
                        uiDisplayInformationManager.DisplayCurrencyInfo(obtainedItemsManager.GetCurrencyList());
                        spinnerResultManager.HideExplodeResultUI();
                        // State
                        currentState = SpinnerState.Initializing;
                    }
                }
                break;
            case SpinnerState.SetRoundToBeginning:
                // Function
                round = 0;
                // UI
                uiDisplayInformationManager.DisplayCurrencyInfo(obtainedItemsManager.GetCurrencyList());
                // State
                currentState = SpinnerState.Initializing;
                break;
            case SpinnerState.ChestsOpening:
                // UI
                if (chestOpenButton.GetFlag())
                {
                    // UI
                    chestOpenButton.SetFlag(false);
                    chestOpenButton.ButtonSetActive(false);
                    wonResultButtonHandler.SetFlag(true);
                    // State
                    currentState = SpinnerState.ChestOpen;
                }
                break;
            case SpinnerState.ChestOpen:
                // UI
                if (wonResultButtonHandler.GetFlag())
                {
                    // Function
                    if (openedChestContents.Count > 0)
                    {
                        // UI
                        wonResultButtonHandler.SetFlag(false);
                        spinnerResultManager.RestartAnimation();
                        spinnerResultManager.DisplaySpinnerResultUI(openedChestContents[0]);
                        // Function
                        openedChestContents.RemoveAt(0);
                        // State
                        currentState = SpinnerState.ChestOpen;
                    }
                    else
                    {
                        // Function
                        if (chestOpenManager.GetThereIsChestInTemp())
                        {
                            // UI
                            spinnerResultManager.HideWonResultUI();
                            wonResultButtonHandler.SetFlag(false);
                            chestOpenButton.ButtonSetActive(true);
                            // Function
                            openedChestContents = chestOpenManager.OpenChest();
                            // State
                            currentState = SpinnerState.ChestsOpening;
                        }
                        else {
                            // UI
                            chestOpenButton.ButtonSetActive(false);
                            chestOpenUIManager.OpenChestUIActive(false);
                            spinnerResultManager.HideWonResultUI();
                            wonResultButtonHandler.SetFlag(false);
                            // Function
                            obtainedItemsManager.MoveTemporaryToPermanentStorage();
                            // State
                            currentState = SpinnerState.SetRoundToBeginning;
                        }
                    }
                }
                break;
        }
    }

    private void InitializeSpinner()
    {
        // Function
        round++;
        string roundType = spinnerWheelUIManager.SpinnerInitialise(round);
        List<Obtainable> spinnerObtainables = spinnerContentManager.GetSpinnerContents(roundType);
        bool isSafeZone = spinnerContentManager.GetCurrentSpinnerIsLeavable();
        numberOfItems = spinnerObtainables.Count;
        spinnerResult = UnityEngine.Random.Range(0, numberOfItems);
        itemResult = spinnerObtainables[spinnerResult];
        // UI
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
