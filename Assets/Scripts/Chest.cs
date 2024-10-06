using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StandaloneItems;

namespace Containers
{
    // note that chest is an obtainable. Obtainable are also Scriptable Object. Thus Chest is a scriptable object.
    [CreateAssetMenu(menuName = "Containers/Chest")]
    public class Chest : Obtainable, IItemContainer
    {
        // Chest is also an obtainable. Thus it has an itemName, image, color.
        [SerializeField]
        private string containerType;
        [SerializeField]
        private int numberOfItemsInChest;
        [SerializeField]
        private List<Obtainable> obtainableItems;
        [SerializeField]
        private List<float> obtainableItemsCoefficients;

        // In order to ensure the obtainableItems and coefficients have the same number of elements when adding
        private int previousCount = 0;


        // Chest-specific clone method
        public Chest Clone(int newAmount)
        {
            // Create a new instance of Chest
            Chest chestClone = ScriptableObject.CreateInstance<Chest>();

            // Copy the Obtainable fields (inherited fields)
            chestClone.itemName = this.itemName;
            chestClone.image = this.image;
            chestClone.colorType = this.colorType;
            chestClone.rarity = this.rarity;
            chestClone.explanation = this.explanation;
            chestClone.amount = newAmount; // Set new amount

            // Copy the chest-specific fields
            chestClone.containerType = this.containerType;
            chestClone.numberOfItemsInChest = this.numberOfItemsInChest;
            chestClone.obtainableItems = new List<Obtainable>(this.obtainableItems); // Shallow copy of items list
            chestClone.obtainableItemsCoefficients = new List<float>(this.obtainableItemsCoefficients); // Shallow copy of coefficients list

            return chestClone;
        }


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


        public List<Obtainable> GetItemsUnlockChest()
        {
            // do this numberOfItemsInChest times and add all picked to the result list
            //      add up all the coefficients
            //      pick random number between 0 (inclusive) to the total of the coefficients (exclusive)
            //      go over each coefficient and find the random number's coefficient's index value
            //      add the obtainable at index value to the list
            // return the list of obtainables
            List<Obtainable> unlockedItems = new List<Obtainable>();
            int noOfItemsUnlockable = obtainableItems.Count;
            float coefficientTotal = 0;
            for (int a = 0; a < noOfItemsUnlockable; a++)
            {
                coefficientTotal += obtainableItemsCoefficients[a];
            }
            for (int i = 0; i < numberOfItemsInChest; i ++)
            {
                float currentTotalCoefficients = 0;
                int currentIndex = 0;
                float randomCoefficient = Random.Range(0, coefficientTotal-1);
                for (int a = 0; a < noOfItemsUnlockable; a++)
                {
                    currentIndex = a;
                    currentTotalCoefficients += obtainableItemsCoefficients[a];
                    if (currentTotalCoefficients > randomCoefficient)
                    {
                        currentIndex = a;
                        a = noOfItemsUnlockable;
                    }
                }
                unlockedItems.Add(obtainableItems[currentIndex]);
            }
            return unlockedItems;
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
