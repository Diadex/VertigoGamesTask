using System.Collections.Generic;
using StandaloneItems;
using UnityEngine;

namespace Containers
{
    public interface IItemContainer
    {
        // type of container: Bronze, gold, big, etc.
        string ContainerType { get;}
        // each unlockable content and its multiplier coefficient.
        // an item with coefficient "z" would have a chance of being unlocked as:
        // the "z" coefficient divided by total of all item coefficients. 1 on default.
        List<Obtainable> ObtainableItems { get;}
        List<float> ObtainableItemsCoefficients { get; }
        // gives the list of obtainable item names along with their coefficients
        List<string> GetItemData();
    }
}