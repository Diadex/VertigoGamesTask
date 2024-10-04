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
public class SpinnerContentManager: MonoBehaviour
{

    [System.Serializable]
    private class SpinnerCategory
    {
        public List<Spinner> spinners;  // Inner list of spinners
        public string spinnerType;
    }

    // list of spinner type, and the spinner objects
    // such as ("gold", [gold_variation1, gold_variation2]), ("silver", [silver_variation1, silver_variation2])...
    [SerializeField]
    private List<SpinnerCategory> spinnerVariations;
    private string obtainableRewardRateWriting;

    private Spinner GetRandomSpinnerVariation(string spinnerTypeName)
    {
        int indexCount = spinnerVariations.Count;
        // Find the tuple with the matching spinnerTypeName
        for (int i = 0; i < indexCount; i++)
        {
            SpinnerCategory pickedSpinnerCategory = spinnerVariations[i];
            if (pickedSpinnerCategory.spinnerType.Equals(spinnerTypeName))
            {
                List<Spinner> spinnerList = pickedSpinnerCategory.spinners;
                // Check if the list is not empty
                if (spinnerList.Count > 0)
                {
                    // Pick a random index
                    int randomIndex = Random.Range(0, spinnerList.Count);
                    Spinner result = spinnerList[randomIndex];
                    obtainableRewardRateWriting = result.GetObtainableRewardRateWriting();
                    return result; // Return the randomly selected spinner
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
    public List<Obtainable> GetSpinnerContents( string spinnerType)
    {
        IItemContainer currentSpinner;
        currentSpinner = GetRandomSpinnerVariation(spinnerType);
        // we pick a number of items from the spinner. Currently all these items are included.
        List<Obtainable> spinnerPossibleItems = currentSpinner.ObtainableItems;
        return spinnerPossibleItems;
    }

    public string GetObtainableRewardRateWriting() {
        return obtainableRewardRateWriting;
    }

}
