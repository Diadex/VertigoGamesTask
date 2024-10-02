using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StandaloneItems;

[CreateAssetMenu(menuName = "Containers/Chest")]
namespace Containers
{
    public class Chest : Obtainable, IItemContainer
    {
        // Chest is also an obtainable. Thus it has an itemName, image, color.
        [SerializeField]
        private string containerType;
        [SerializeField]
        private List<(Obtainable obtainable, int coefficient)> obtainableItems;


        // Explicit interface implementation for the IItemContainer defined variables/functions
        string IItemContainer.ContainerType => containerType;
        List<(Obtainable obtainable, int coefficient)> IItemContainer.ObtainableItems => obtainableItems;
        List<string> IItemContainer.GetItemData()
        {
            List<string> itemData = new List<string>();
            foreach (var item in obtainableItems)
            {
                itemData.Add($"{containerType} {item.obtainable.name} (Coefficient: {item.coefficient})");
            }
            return itemData;
        }
    }
}
