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
    private GameObject[] slotUIGameObj;
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
            DisplaceUIElement(slotUIGameObj[i], i * degreesInBetween * Mathf.Deg2Rad);

            // then set the values in each slotUIGameObj's image and text field according to the slotObtainableItems
            Obtainable currentItem = slotObtainableItems[i];
            Image imageComponent = slotUIGameObj[i].GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = currentItem.GetImage(); // Assuming Obtainable has an 'image' property of type Sprite
                imageComponent.color = currentItem.GetColor(); // Assuming Obtainable has a 'color' property of type Color
            }
        }
    }

    private void DisplaceUIElement( GameObject gameObj, float degreesTurned)
    {
        // Calculate the new position based on the angle and distance from the center
        float xPos = Mathf.Cos(degreesTurned) * slotUIDistanceFromCenter;
        float yPos = Mathf.Sin(degreesTurned) * slotUIDistanceFromCenter;

        // Set the position and rotation of the UI slot
        gameObj.transform.localPosition = new Vector3(xPos, yPos, 0);
        gameObj.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(xPos, yPos, 0));
    }


}
