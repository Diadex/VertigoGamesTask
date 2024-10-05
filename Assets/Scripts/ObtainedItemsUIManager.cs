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
    private RectTransform prefabGrouperUIPerma;
    [SerializeField]
    private RectTransform prefabGrouperUITemp;
    [SerializeField]
    private float singleRowHeight = 150f;
    [SerializeField]
    private int columnCount = 6;

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
        // update the height of the storageUI's
        if (obtainedSize > 0)
        {
            float heightAmount = (((obtainedSize-1) / columnCount)+1) * singleRowHeight;

            if (storageType == ObtainedItemsManager.StorageType.PermaStorage)
            {
                if (prefabGrouperUIPerma != null)
                {
                    Vector2 size = prefabGrouperUIPerma.sizeDelta;
                    prefabGrouperUIPerma.sizeDelta = new Vector2(size.x, heightAmount);
                }
            }
            else
            {
                if (prefabGrouperUITemp != null)
                {
                    Vector2 size = prefabGrouperUITemp.sizeDelta;
                    prefabGrouperUITemp.sizeDelta = new Vector2(size.x, heightAmount);
                }
            }
        }

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
