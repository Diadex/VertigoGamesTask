using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Containers;
using StandaloneItems;

/*
Depending on which round it is, define what will go in the spinner. 
So, get the data from the scriptable objects for gold, silver, bronze. 
Spinner manager will have these scriptable objects in an array for the three types of spinners.
*/
public class SpinnerContentManager
{
    // list of spinner type, and the spinner objects
    // such as ("gold", [gold_variation1, gold_variation2]), ("silver", [silver_variation1, silver_variation2])...
    [SerializeField]
    private List<(string spinnerTypeName, List<Spinner> spinners)> SpinnerVariations;
    // TODO change the round count from gameManager. Currently in editor
    [SerializeField]
    private int round = 1;

    private Spinner GetRandomSpinnerVariation(string spinnerTypeName)
    {
        // Find the tuple with the matching spinnerTypeName
        foreach (var variation in SpinnerVariations)
        {
            if (variation.spinnerTypeName == spinnerTypeName)
            {
                List<Spinner> spinnerList = variation.spinners;
                // Check if the list is not empty
                if (spinnerList.Count > 0)
                {
                    // Pick a random index
                    int randomIndex = Random.Range(0, spinnerList.Count);
                    return spinnerList[randomIndex]; // Return the randomly selected spinner
                }
                else
                {
                    Debug.LogWarning($"The {spinnerTypeName} Type Spinner list is empty in SpinnerContentManager");
                    return null;
                }
            }
        }

        Debug.LogWarning($"Spinner type '{spinnerTypeName}' not found in SpinnerContentManager.");
        return null;
    }

    // TODO does chest data work this way?
    public List<Obtainable> GetSpinnerContents()
    {
        IItemContainer currentSpinner;
        // gold
        if (round % 30 == 0)
        {
            currentSpinner = GetRandomSpinnerVariation("gold");
        }// silver
        else if (round % 5== 0)
        {
            currentSpinner = GetRandomSpinnerVariation("silver");
        }// bronze
        else
        {
            currentSpinner = GetRandomSpinnerVariation("bronze");
        }
        // we pick a number of items from the spinner. Currently all these items are included.
        List<Obtainable> spinnerPossibleItems = currentSpinner.ObtainableItems;
        return spinnerPossibleItems;
    }

}
