using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StandaloneItems;

namespace Containers
{
    [CreateAssetMenu(menuName = "Containers/Spinner")]
    public class Spinner : ScriptableObject, IItemContainer
    {
        [SerializeField]
        private string containerType;
        [SerializeField]
        private List<Obtainable> obtainableItems;
        [SerializeField]
        private List<float> obtainableItemsCoefficients;


        // In order to ensure the obtainableItems and coefficients have the same number of elements when adding
        private int previousCount = 0;

        // Explicit interface implementation for the IItemContainer defined variables/functions
        string IItemContainer.ContainerType => containerType;
        List<Obtainable> IItemContainer.ObtainableItems => obtainableItems;
        List<float> IItemContainer.ObtainableItemsCoefficients => obtainableItemsCoefficients;
        List<string> IItemContainer.GetItemData()
        {
            int size = obtainableItems.Count;
            List<string> itemData = new List<string>();
            for (int a = 0; a < size; a++)
            {
                itemData.Add($"{containerType} {obtainableItems[a].name} (Coefficient: {obtainableItemsCoefficients[a]})");
            }
            return itemData;
        }











        // Method to update the editor for the coefficients and the obtainableItems

        private void OnValidate()
        {

            // Handle newly added items
            if (obtainableItems.Count > previousCount)
            {
                // Add coefficients if there are more items
                while (obtainableItems.Count > obtainableItemsCoefficients.Count)
                {
                    obtainableItemsCoefficients.Add(1f); // Default coefficient to 1
                }
            }

            // Update the previous count to the current count
            previousCount = obtainableItems.Count;
        }
    }
}