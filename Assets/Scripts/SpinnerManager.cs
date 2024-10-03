using StandaloneItems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// coordinates the spinner flow
public class SpinnerManager : MonoBehaviour
{
    [SerializeField]
    private SpinnerContentManager spinnerContentManager;
    [SerializeField]
    private SpinnerSlotPlacer spinnerSlotPlacer;
    [SerializeField]
    private SpinnerWheelUIManager spinnerWheelUIManager;
    [SerializeField]
    private SpinnerButtonHandler spinnerButtonHandler;
    [SerializeField]
    private SpinnerAnimator spinnerAnimator;
    [SerializeField]
    private SpinnerResultManager spinnerResultManager;

    [SerializeField] // TODO remove the serializefield here
    private int round = 0;
    private bool spinButtonPressed = false;
    private bool spinnerInitialised = false;
    private int spinnerResult = 0;
    private Obtainable itemResult;

    private void Update()
    {
        // put the UI elements to place according to the round
        // check if spin button is pressed. If the flag is true, set it to false.
        // set spinButtonPressed as true and get the spinner result.
        // animate the spinner and stop it at the correct location
        // later show the next screen and such
        if (!spinnerInitialised)
        {
            InitializeSpinner();
        }
        else if (!spinButtonPressed)
        {
            // check the button
            if (spinnerButtonHandler.GetFlag())
            {
                spinnerButtonHandler.SetFlag(false);
                spinButtonPressed = true;
            }
        }
        else
        { // button pressed, await to become activated after spinning and showing is over.
            Debug.Log("Spinner result is " + spinnerResult + ", so it is " + itemResult.GetAmount() + " " + itemResult.GetName() + " obtainable");
            spinnerInitialised = false;
            spinButtonPressed = false;
        }
    }

    private void InitializeSpinner()
    {
        round++;
        string roundType = spinnerWheelUIManager.SpinnerRoundType(round);
        List<Obtainable> spinnerObtainables = spinnerContentManager.GetSpinnerContents(roundType);
        spinnerSlotPlacer.SetSlotObtainableItems(spinnerObtainables);
        int numberOfItems = spinnerObtainables.Count;
        spinnerResult = UnityEngine.Random.Range(0, numberOfItems);
        itemResult = spinnerObtainables[spinnerResult];
        spinnerInitialised = true;
    } 

}
