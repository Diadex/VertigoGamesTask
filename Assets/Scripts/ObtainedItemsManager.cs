using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StandaloneItems;
using Containers;

public class ObtainedItemsManager : MonoBehaviour
{
    // we have a temporary obtained items
    // we also have a permanent obtained items list
    // the items are Obtainable items which contain the name of the item, its image, its amount, color, rarity, explanation
    [SerializeField]
    private List<Obtainable> temporaryStorage;
    [SerializeField]
    private List<Obtainable> permaStorage;
    [SerializeField]
    private List<Obtainable> allStorableObtainablesList;
    [SerializeField]
    private string cashName = "Cash";
    [SerializeField]
    private string goldName = "Gold";

    // int is 1 if temp, 2 if perma storage
    public enum StorageType
    {
        TempStorage = 1,
        PermaStorage = 2
    }

    private void Start()
    {
        LoadPermanentStorage();
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

    private bool StackItemToList(List<Obtainable> storage, Obtainable addingItem)
    {
        int storageSize = storage.Count;
        for (int i = 0; i < storageSize; i++)
        {
            Obtainable storageItem = storage[i];
            if (storageItem.GetName().Equals(addingItem.GetName()))
            {
                Obtainable copiedItem = CloneObtainable(storageItem, storageItem.GetAmount() + addingItem.GetAmount());
                storage.RemoveAt(i);
                storage.Add(copiedItem);
                return true;
            }
        }
        return false;
    }

    // Clone the obtainable item (or chest) with a new amount
    private Obtainable CloneObtainable(Obtainable storageItem, int amount)
    {
        // Check if the storage item is a Chest
        if (storageItem is Chest chestItem)
        {
            // Clone using the Chest's Clone method
            return chestItem.Clone(amount);
        }
        else
        {
            // Fallback to the Obtainable's Clone method
            return Obtainable.Clone(storageItem,amount);
        }
    }

    public void MoveTemporaryToPermanentStorage()
    {
        int tempStorageSize = temporaryStorage.Count;
        for (int i = 0; i < tempStorageSize; i++)
        {
            Obtainable currentItem = temporaryStorage[i];
            if (currentItem.GetAmount() >= 0)
            {
                AddItemToStorage(StorageType.PermaStorage, currentItem);
            }
        }
        ClearTempStorage();
        SavePermanentStorage();
    }



    public void ClearTempStorage()
    {
        temporaryStorage.Clear();
    }

    // returns false when there isn't enough gold currency
    public bool SpendGold( int spentAmount)
    {
        int permaStorageSize = permaStorage.Count;
        for (int i = 0; i < permaStorageSize; i++)
        {
            if (permaStorage[i].GetName().Equals(goldName))
            {
                Obtainable gold = permaStorage[i];
                if (gold != null && gold.GetAmount() >= spentAmount)
                {
                    Obtainable goldCloned = CloneObtainable(gold, gold.GetAmount() -spentAmount);
                    permaStorage.RemoveAt(i);
                    permaStorage.Add(goldCloned);
                    return true;
                }
            }
        }
        return false;
    }

    public List<(string currency, int amount)> GetCurrencyList()
    {
        List<(string currency, int amount)> result = new List<(string currency, int amount)> { (cashName, 0), (goldName, 0) };

        int permaStorageSize = permaStorage.Count;
        for (int i = 0; i < permaStorageSize; i++)
        {
            Obtainable current = permaStorage[i];
            if (current.GetName().Equals(cashName))
            {
                result[0] = (cashName, current.GetAmount());
            }
            else if (current.GetName().Equals(goldName))
            {
                result[1] = (goldName, current.GetAmount());
            }
        }
        return result;
    }

    // return a copy of permastorage
    public List<Obtainable> GetStorageList( StorageType storage)
    {
        if (storage == StorageType.PermaStorage)
        {
            return new List<Obtainable>(permaStorage);
        }
        else
        {
            return new List<Obtainable>(temporaryStorage);
        }
    }

    public void RemoveFromList( StorageType storage, Obtainable item)
    {
        if (storage == StorageType.PermaStorage)
        {
            permaStorage.Remove(item);
        }
        else
        {
            temporaryStorage.Remove(item);
        }
    }

    private void SavePermanentStorage()
    {
        List<string> savedItems = new List<string>();

        foreach (var item in permaStorage)
        {
            savedItems.Add(item.GetName() + ":" + item.GetAmount());
        }
        string recordedString = string.Join(",", savedItems);
        PlayerPrefs.SetString("PermanentStorage", recordedString);
        PlayerPrefs.Save();
    }

    public void ClearPermanentStorage()
    {
        PlayerPrefs.SetString("PermanentStorage", "Gold:100,Cash:1000");
        PlayerPrefs.Save();
    }

    private void LoadPermanentStorage()
    {
        if (PlayerPrefs.HasKey("PermanentStorage"))
        {
            string savedData = PlayerPrefs.GetString("PermanentStorage");
            string[] items = savedData.Split(',');

            foreach (var itemData in items)
            {
                string[] parts = itemData.Split(':');
                string itemName = parts[0];
                int itemAmount = int.Parse(parts[1]);
                // Find the corresponding Obtainable object, then add it to permaStorage
                Obtainable obtainableItem = GetObtainable(itemName);
                if (obtainableItem != null)
                {
                    Obtainable copiedItem = CloneObtainable(obtainableItem, itemAmount);
                    permaStorage.Add(copiedItem);
                }
                else
                {
                    Debug.Log("Error: The Obtainable item with a " + itemData+ " itemData is not included in the list. ObtainedItemsManager.");
                }
            }
        }
    }

    private Obtainable GetObtainable(string itemName)
    {
        int permaStorageSize = allStorableObtainablesList.Count;
        for (int i = 0; i < permaStorageSize; i++)
        {
            if (allStorableObtainablesList[i].GetName().Equals(itemName))
            {
                return allStorableObtainablesList[i];
            }
        }
        return null;
    }

}
