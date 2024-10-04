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
    private ButtonHandler spinnerButtonHandler;
    [SerializeField]
    private ButtonHandler leaveButtonHandler;
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

    private enum SpinnerState
    {
        Initializing,
        WaitingForButtonPress,
        Spinning,
        ResultDisplayed,
        ObtainableItemsDisplayed,
        Exploding,
        SetRoundToBeginning,
    }

    private SpinnerState currentState;

    private int round = 0;
    private int spinnerResult = 0;
    private int numberOfItems;
    private Obtainable itemResult;
    private GameObject[] spinnerWheels;

    private void Start()
    {
        spinnerWheels = spinnerWheelUIManager.GetSpinnerWheelUIReferences();
        currentState = SpinnerState.SetRoundToBeginning;
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

                    obtainedItemsManager.MoveTemporaryToPermanentStorage();
                    // could potentially add a display all won results state
                    currentState = SpinnerState.SetRoundToBeginning;
                }
                break;

            case SpinnerState.Spinning:
                if (!spinnerAnimator.CheckAnimation())
                {
                    Debug.Log("Spinner result is " + spinnerResult + ", so it is " + itemResult.GetAmount() + " " + itemResult.GetName() + " obtainable");
                    obtainedItemsManager.AddItemToStorage(ObtainedItemsManager.StorageType.TempStorage, itemResult);
                    currentState = SpinnerState.ResultDisplayed;
                }
                break;

            case SpinnerState.SetRoundToBeginning:
                round = 0;
                uiDisplayInformationManager.DisplayCurrencyInfo(obtainedItemsManager.GetCurrencyList());
                currentState = SpinnerState.Initializing;
                break;

            case SpinnerState.ResultDisplayed:
                // display the won item in that round.
                currentState = SpinnerState.Initializing;
                break;

        }
    }

    private void InitializeSpinner()
    {
        Debug.Log("Initialized");
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
        
        spinnerPanelUIManager.SetSpinWriting(roundType, spinnerContentManager.GetObtainableRewardRateWriting());

        spinnerButtonHandler.ButtonSetActive(true);
        leaveButtonHandler.ButtonSetActive(true);
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
