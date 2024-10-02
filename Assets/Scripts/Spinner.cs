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
        private List<(Obtainable obtainable, int coefficient)> obtainableItems;

        // Track the previous count of obtainable items
        private int previousCount = 0;

        // Explicit interface implementation for the IItemContainer defined variables/functions
        string IItemContainer.ContainerType => containerType;
        List<(Obtainable obtainable, int coefficient)> IItemContainer.ObtainableItems => obtainableItems;
        List<string> IItemContainer.GetItemData()
        {
            List<string> itemData = new List<string>();
            foreach (var item in obtainableItems)
            {
                itemData.Add($"{item.obtainable.name} (Coefficient: {item.coefficient})");
            }
            return itemData;
        }


        // The newly added spinner coefficients are defaulted to 1. The coefficients are included for scalability.
        private void OnValidate()
        {
            // Ensure the list is initialized
            if (obtainableItems != null)
            {
                // Check if a new item has been added
                if (obtainableItems.Count > previousCount)
                {
                    // Set the coefficient of the new item to 1
                    int newIndex = obtainableItems.Count - 1; // Get the index of the newly added item
                    var newItem = obtainableItems[newIndex];
                    obtainableItems[newIndex] = (newItem.obtainable, 1); // Set coefficient to 1
                }

                // Update the previous count to the current count
                previousCount = obtainableItems.Count;
            }
        }
    }
}