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
    private SpinnerAnimator spinnerAnimator;
    [SerializeField]
    private SpinnerResultManager spinnerResultManager;
    [SerializeField]
    private UIDisplayInformationManager uiInfoManager;
    [SerializeField]
    private SpinnerPanelUIManager spinnerPanelUIManager;
    [SerializeField]
    private ObtainedItemsManager obtainedItemsManager;

    private enum SpinnerState
    {
        Initializing,
        WaitingForSpin,
        Spinning,
        ResultDisplayed
    }

    private SpinnerState currentState = SpinnerState.Initializing;

    private int round = 0;
    private int spinnerResult = 0;
    private int numberOfItems;
    private Obtainable itemResult;
    private GameObject[] spinnerWheels;

    private void Start()
    {
        spinnerWheels = spinnerWheelUIManager.GetSpinnerWheelUIReferences();
    }

    private void Update()
    {
        switch (currentState)
        {
            case SpinnerState.Initializing:
                InitializeSpinner();
                break;

            case SpinnerState.WaitingForSpin:
                if (spinnerButtonHandler.GetFlag())
                {
                    spinnerButtonHandler.ButtonSetActive(false);
                    spinnerAnimator.SpinWheels(spinnerWheels, spinnerResult, numberOfItems);
                    currentState = SpinnerState.Spinning;
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

            case SpinnerState.ResultDisplayed:

                spinnerButtonHandler.SetFlag(false);
                currentState = SpinnerState.Initializing;
                break;
        }
    }

    private void InitializeSpinner()
    {
        Debug.Log("Initialized");
        round++;
        uiInfoManager.SetRoundText(round);
        string roundType = spinnerWheelUIManager.SpinnerInitialise(round);
        List<Obtainable> spinnerObtainables = spinnerContentManager.GetSpinnerContents(roundType);
        spinnerSlotPlacer.SetSlotObtainableItems(spinnerObtainables);
        numberOfItems = spinnerObtainables.Count;
        spinnerResult = UnityEngine.Random.Range(0, numberOfItems);
        itemResult = spinnerObtainables[spinnerResult];
        spinnerPanelUIManager.SetSpinWriting(roundType, spinnerContentManager.GetObtainableRewardRateWriting());

        currentState = SpinnerState.WaitingForSpin;
        spinnerButtonHandler.ButtonSetActive(true);
    }
}
