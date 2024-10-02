using StandaloneItems;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

    private void Start()
    {
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
            // rotate the item, then move it in the rotated direction by slotUIDistanceFromCenter.
            // then set the values in each slotUIGameObj's image and text field according to the slotObtainableItems

            DisplaceUIElement(slotUIGameObj[i], i * degreesInBetween * Mathf.Deg2Rad);

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
