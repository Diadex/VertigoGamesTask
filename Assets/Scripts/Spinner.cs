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

    }
}