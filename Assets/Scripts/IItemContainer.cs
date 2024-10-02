using System.Collections.Generic;

namespace Containers
{
    public interface IItemContainer
    {
        // type of container: Bronze, gold, big, etc.
        string ContainerType { get;}
        // each unlockable content and its multiplier coefficient.
        // an item with coefficient "z" would have a chance of being unlocked as:
        // the "z" coefficient divided by total of all item coefficients. 1 on default.
        List<(Obtainable obtainable, int coefficient)> ObtainableItems { get;}
        // gives the list of obtainable item names along with their coefficients
        List<string> GetItemData();
    }
}