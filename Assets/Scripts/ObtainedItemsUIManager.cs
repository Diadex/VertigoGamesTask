using StandaloneItems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainedItemsUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject uiElementObtainedItemsDisplay;
    [SerializeField]
    private ObtainedItemsManager obtainedItemsManager;
    [SerializeField]
    private GameObject obtainedItemPrefab;
    [SerializeField]
    private Transform prefabGrouperUIPerma;
    [SerializeField]
    private Transform prefabGrouperUITemp;

    public void EnableUI(bool isEnabled)
    {
        uiElementObtainedItemsDisplay.SetActive(isEnabled);
    }
    
    public void SetUIObtainedStorage()
    {
        SetUIStorage(prefabGrouperUIPerma, ObtainedItemsManager.StorageType.PermaStorage);
        SetUIStorage(prefabGrouperUITemp, ObtainedItemsManager.StorageType.TempStorage);
    }

    private void SetUIStorage(Transform prefabGrouperUITransform, ObtainedItemsManager.StorageType storageType)
    {
        List<Obtainable> obtainedItemsList = obtainedItemsManager.GetStorageList(storageType);
        int obtainedSize = obtainedItemsList.Count;
        int childCount = prefabGrouperUITransform.childCount;

        // Update existing children
        UpdateExistingChildren(obtainedItemsList, obtainedSize, childCount, prefabGrouperUITransform);

        // Instantiate new prefabs if necessary
        InstantiateNewPrefabs(obtainedItemsList, obtainedSize, childCount, prefabGrouperUITransform);

        // Remove any excess prefabs
        RemoveExcessChildren(obtainedSize, childCount, prefabGrouperUITransform);
    }

    private void UpdateExistingChildren(List<Obtainable> obtainedItemsList, int obtainedSize, int childCount, Transform prefabGrouperUI)
    {
        for (int i = 0; i < Mathf.Min(obtainedSize, childCount); i++)
        {
            Transform child = prefabGrouperUI.GetChild(i);
            ItemDisplayUIManager itemDisplay = child.GetComponent<ItemDisplayUIManager>();

            if (itemDisplay == null)
            {
                Debug.LogWarning("Error: prefab child does not have an ItemDisplayUIManager script!");
            }
            else
            {
                // Update the existing prefab with new data
                itemDisplay.SetImageAndText(obtainedItemsList[i]);
            }
        }
    }

    private void InstantiateNewPrefabs(List<Obtainable> obtainedItemsList, int obtainedSize, int childCount, Transform prefabGrouperUI)
    {
        for (int i = childCount; i < obtainedSize; i++)
        {
            GameObject newPrefab = Instantiate(obtainedItemPrefab);

            // Set the parent to the specified GameObject and adjust the local scale/position
            newPrefab.transform.SetParent(prefabGrouperUI);
            newPrefab.transform.localScale = Vector3.one;
            newPrefab.transform.localPosition = Vector3.zero;

            ItemDisplayUIManager itemDisplay = newPrefab.GetComponent<ItemDisplayUIManager>();
            if (itemDisplay == null)
            {
                Debug.LogWarning("Error: prefab does not have an ItemDisplayUIManager script!");
            }
            else
            {
                // Set data for the new prefab
                itemDisplay.SetImageAndText(obtainedItemsList[i]);
            }
        }
    }

    private void RemoveExcessChildren(int obtainedSize, int childCount, Transform prefabGrouperUI)
    {
        for (int i = obtainedSize; i < childCount; i++)
        {
            Transform child = prefabGrouperUI.GetChild(i);
            Destroy(child.gameObject);
        }
    }


}
