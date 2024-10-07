using StandaloneItems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Coordinates the spinner flow
public class SpinnerManager : MonoBehaviour
{
    [SerializeField]
    private SpinnerAnimator spinnerAnimator;
    [SerializeField]
    private SpinnerResultManager spinnerResultManager;
    [SerializeField]
    private ObtainedItemsManager obtainedItemsManager;
    [SerializeField]
    private ChestOpenManager chestOpenManager;
    [SerializeField]
    private SpinnerUIManager spinnerUIManager;

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
        // UI
        spinnerWheels = spinnerUIManager.GetSpinnerWheelsReferences();
        // State
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
                if (spinnerUIManager.GetIsSpinClicked())
                {
                    // UI
                    spinnerUIManager.SpinnerButtonClicked();
                    // Function
                    spinnerAnimator.SpinWheels(spinnerWheels, spinnerResult, numberOfItems);
                    // State
                    currentState = SpinnerState.Spinning;
                }// UI
                else if (spinnerUIManager.GetIsLeaveClicked())
                {
                    // UI
                    spinnerUIManager.LeaveButtonPressed();
                    // Function (save)
                    chestOpenManager.SaveChestsToOpenManager();
                    // Function (chests)
                    if (chestOpenManager.GetThereIsChestInTemp())
                    {
                        // UI
                        spinnerUIManager.BeginChestOpening();
                        // Function
                        openedChestContents = chestOpenManager.OpenChest();
                        // State
                        currentState = SpinnerState.ChestsOpening;
                    }
                    else
                    {
                        // UI
                        spinnerUIManager.ChestOpenButtonDisable();
                        // Function
                        obtainedItemsManager.MoveTemporaryToPermanentStorage();
                        // State
                        currentState = SpinnerState.SetRoundToBeginning;
                    }
                }// UI
                else if (spinnerUIManager.GetIsObtainedItemsOpenClicked())
                {
                    // UI
                    spinnerUIManager.OpenObtainedItems();
                    // State
                    currentState = SpinnerState.ObtainableItemsDisplayed;
                }
                // State
                if (currentState != SpinnerState.WaitingForButtonPress)
                {
                    //UI
                    spinnerUIManager.ObtainableItemsButtonSetDisabled();
                }
                break;
            case SpinnerState.ObtainableItemsDisplayed:
                // UI
                if (spinnerUIManager.GetIsObtainedItemsExitClicked())
                {
                    // UI
                    spinnerUIManager.ObtainedItemsExited();
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
                    if (spinnerUIManager.GetIsExplodeAndDisplayUI(itemResult))
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
                if (spinnerUIManager.GetIsCloseResultViewClicked())
                {
                    // UI
                    spinnerUIManager.ExitWonResult();
                    // State
                    currentState = SpinnerState.Initializing;
                }
                break;
            case SpinnerState.Exploded:
                // UI
                if (spinnerUIManager.GetIsExplodeGiveUpClicked())
                {
                    // UI
                    spinnerUIManager.ReEnableGiveUpResultButton();
                    spinnerUIManager.HideExplodeUI();
                    // Function (save)
                    obtainedItemsManager.ClearTempStorage();
                    // State
                    currentState = SpinnerState.SetRoundToBeginning;
                }// UI
                else if (spinnerUIManager.GetIsExplodeReviveClicked())
                {
                    // UI
                    spinnerUIManager.ReEnableExplodeResultButton();
                    // Function
                    int reviveCostGold = spinnerResultManager.GetCostOnExplosion();
                    // spend gold, update the gold UI, then continue from the next round
                    if (obtainedItemsManager.SpendGold(reviveCostGold))
                    {
                        // UI
                        spinnerUIManager.DisplayCurrencyInfo(obtainedItemsManager.GetCurrencyList());
                        spinnerUIManager.HideExplodeUI();
                        // State
                        currentState = SpinnerState.Initializing;
                    }
                }
                break;
            case SpinnerState.SetRoundToBeginning:
                // Function
                round = 0;
                // UI
                spinnerUIManager.DisplayCurrencyInfo(obtainedItemsManager.GetCurrencyList());
                // State
                currentState = SpinnerState.Initializing;
                break;
            case SpinnerState.ChestsOpening:
                // UI
                if (spinnerUIManager.GetIsChestOpenClicked())
                {
                    // UI
                    spinnerUIManager.ChestOpenDeactivate();
                    // State
                    currentState = SpinnerState.ChestOpen;
                }
                break;
            case SpinnerState.ChestOpen:
                // UI
                if (spinnerUIManager.GetIsCloseResultViewClicked())
                {
                    // Function
                    if (openedChestContents.Count > 0)
                    {
                        // UI
                        spinnerUIManager.ContinueWithNextItem(openedChestContents[0]);
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
                            spinnerUIManager.ContinueChestOpeningNextChest();
                            // Function
                            openedChestContents = chestOpenManager.OpenChest();
                            // State
                            currentState = SpinnerState.ChestsOpening;
                        }
                        else
                        {
                            //UI
                            spinnerUIManager.EndChestOpeningAndReset();
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
        string roundType = spinnerUIManager.InitSpinnerGetRoundType(round);
        List<Obtainable> spinnerObtainables = spinnerUIManager.GetSpinnerContents(roundType);
        bool isSafeZone = spinnerUIManager.GetIsSafeZone();
        numberOfItems = spinnerObtainables.Count;
        spinnerResult = UnityEngine.Random.Range(0, numberOfItems);
        itemResult = spinnerObtainables[spinnerResult];
        // UI
        spinnerUIManager.InitializeSpinner(round, isSafeZone, spinnerObtainables, roundType);
    }

}
