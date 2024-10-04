using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StandaloneItems;

public class ObtainedItemsManager : MonoBehaviour
{
    // we have a temporary obtained items
    // we also have a permanent obtained items list
    // the items are Obtainable items which contain the name of the item, its image, its amount, color, rarity, explanation
    [SerializeField]
    private List<Obtainable> temporaryStorage;
    [SerializeField]
    private List<Obtainable> permaStorage;

    // int is 1 if temp, 2 if perma storage
    public enum StorageType
    {
        TempStorage = 1,
        PermaStorage = 2
    }

    public void AddItemToStorage( StorageType storageType, Obtainable obtainable)
    {
        // check if the storage has the item
        // stack it if it has
        // else, add it to its list
        List<Obtainable> storage = storageType == StorageType.TempStorage ? temporaryStorage : permaStorage;
        bool hasStackedToList = StackItemToList(storage, obtainable);
        if (!hasStackedToList)
        {
            storage.Add(obtainable);
        }
    }

    private bool StackItemToList(List<Obtainable> storage, Obtainable obtainable)
    {
        int storageSize = storage.Count;
        for (int i = 0; i < storageSize; i++)
        {
            Obtainable storageItem = storage[i];
            if (storageItem.GetName().Equals(obtainable.GetName()))
            {
                Obtainable copiedItem = new Obtainable(storageItem, storageItem.GetAmount() + obtainable.GetAmount());
                storage.RemoveAt(i);
                storage.Add(copiedItem);
                return true;
            }
        }
        return false;
    }

}
