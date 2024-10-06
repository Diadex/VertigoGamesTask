using Containers;
using StandaloneItems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpenManager : MonoBehaviour
{
    [SerializeField]
    private ObtainedItemsManager obtainedItems;
    [SerializeField]
    private ChestOpenUIManager chestOpenUIManager;
    private List<Chest> savedTempChests;


    public void SaveChestsToOpenManager()
    {
        savedTempChests = RetrieveTempStorageChests();
    }

    public bool GetThereIsChestInTemp()
    {
        return (savedTempChests != null && savedTempChests.Count > 0);
    }

    public List<Obtainable> OpenChest()
    {
        chestOpenUIManager.UpdateChestOpenUI(savedTempChests[0]);
        List<Obtainable> list = savedTempChests[0].GetItemsUnlockChest();
        savedTempChests.RemoveAt(0);
        for (int i = 0; i < list.Count; i++)
        {
            obtainedItems.AddItemToStorage(ObtainedItemsManager.StorageType.TempStorage, list[i]);
        }
        return new List<Obtainable>(list);
    }

    private List<Chest> RetrieveTempStorageChests()
    {
        List<Chest> resultChests = new List<Chest>();
        // go over obtainedItems' temp storage and return the items which are chest type
        List<Obtainable> allItemsInTemp = obtainedItems.GetStorageList(ObtainedItemsManager.StorageType.TempStorage);
        int storageSize = allItemsInTemp.Count;
        for (int i = 0; i < storageSize; i++)
        {
            if (allItemsInTemp[i] is Chest chestItem)
            {
                int noOfChests = chestItem.GetAmount();
                for (int a = 0; a < noOfChests; a++)
                {
                    Chest newChest = chestItem.Clone( 1);
                    resultChests.Add(newChest);
                }
                obtainedItems.RemoveFromList(ObtainedItemsManager.StorageType.TempStorage, chestItem);
            }
        }
        return resultChests;
    }


}
