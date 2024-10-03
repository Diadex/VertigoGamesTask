using StandaloneItems;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// places spinner's obtainables to specific locations on the UI.
public class SpinnerSlotPlacer : MonoBehaviour
{
    [SerializeField]
    private List<Obtainable> slotObtainableItems;
    // we have 8 currently, but if a spinner had more, we could just add another in the editor
    [SerializeField]
    private GameObject[] slotUIGameObjects;
    // If a spinner's UI is different such that the distance is further
    [SerializeField]
    private float slotUIDistanceFromCenter = 144f;


    public void SetSlotObtainableItems(List<Obtainable> obtainableList)
    {
        slotObtainableItems = obtainableList;
        SetItemsUI();
    }

    private void SetItemsUI()
    {
        // place these slots to their respective places.
        int numberOfItems = 8;//slotObtainableItems.Count;
        float degreesInBetween = 360f / numberOfItems;
        for (int i = 0; i < numberOfItems; i++)
        {
            // for each i of the slotUIGameObj, adjust its transform.
            DisplaceUIElement(slotUIGameObjects[i], i * degreesInBetween * Mathf.Deg2Rad);

            // then set the values in each slotUIGameObj's image and text field according to the slotObtainableItems
            Obtainable currentItem = slotObtainableItems[i];
            SlotUIManager slotInformation = slotUIGameObjects[i].GetComponent<SlotUIManager>();
            if (slotInformation != null)
            {
                slotInformation.SetImageAndText(currentItem);
            }
        }
    }


    private void DisplaceUIElement( GameObject gameObj, float degreesTurned)
    {
        //gameObj.transform.rotation = Quaternion.identity;

        // Calculate the new position based on the angle and distance from the center
        float xPos = Mathf.Sin(degreesTurned) * slotUIDistanceFromCenter;
        float yPos = Mathf.Cos(degreesTurned) * slotUIDistanceFromCenter;

        // Set the position and rotation of the UI slot
        gameObj.transform.localPosition = new Vector3(xPos, yPos, 0);
        gameObj.transform.localRotation = Quaternion.LookRotation(Vector3.forward, new Vector3(xPos, yPos, 0));
    }


}
