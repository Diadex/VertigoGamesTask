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
    private int numberOfItems;
    private Obtainable itemResult;
    private GameObject[] spinnerWheels;

    private void Start()
    {
        spinnerWheels = spinnerWheelUIManager.GetSpinnerWheelUIReferences();
    }


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
            if (spinnerButtonHandler.GetFlag() && !spinnerAnimator.CheckAnimation())
            {
                spinnerButtonHandler.SetFlag(false);
                spinButtonPressed = true;
                spinnerAnimator.SpinWheels(spinnerWheels, spinnerResult * 360 / numberOfItems);
            }
        }
        else if (spinButtonPressed)
        {
            if (!spinnerAnimator.CheckAnimation())
            {
                Debug.Log("Spinner result is " + spinnerResult + ", so it is " + itemResult.GetAmount() + " " + itemResult.GetName() + " obtainable");
                spinnerInitialised = false;
                spinButtonPressed = false;
            }
        }
    }

    private void InitializeSpinner()
    {
        Debug.Log("Initialised");
        round++;
        string roundType = spinnerWheelUIManager.SpinnerInitialise(round);
        List<Obtainable> spinnerObtainables = spinnerContentManager.GetSpinnerContents(roundType);
        spinnerSlotPlacer.SetSlotObtainableItems(spinnerObtainables);
        numberOfItems = spinnerObtainables.Count;
        spinnerResult = UnityEngine.Random.Range(0, numberOfItems);
        itemResult = spinnerObtainables[spinnerResult];
        spinnerInitialised = true;
    } 

}
