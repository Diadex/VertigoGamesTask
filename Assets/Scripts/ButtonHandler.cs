using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] // The active state button
    private Button buttonActive;
    [SerializeField] // The disabled button GameObject (this is an optional non-clickable image)
    private GameObject buttonDisabledGameObj;

    // flag true when the button is pressed.
    private bool flag;
    private bool isActive = true;

    private void OnValidate()
    {
        // Automatically assign the Button component if it's not set
        if (buttonActive == null)
        {
            buttonActive = GetComponentInChildren<Button>(); // Finds Button on child if missing
        }
    }

    private void Update()
    {
        if (isActive && Input.GetMouseButtonDown(0))
        {
            // check if mouse is over the button's border as we clicked
            RectTransform buttonRect = buttonActive.GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(buttonRect, Input.mousePosition, null))
            {
                SetFlag(true);
            }
        }
    }

    public void ButtonSetActive(bool isActiveVar)
    {
        if (buttonActive != null)
        {
            buttonActive.gameObject.SetActive(isActiveVar);
        }

        if (buttonDisabledGameObj != null)
        {
            buttonDisabledGameObj.SetActive(!isActiveVar);
        }

        isActive = isActiveVar;
    }

    // Get and set for flag
    public bool GetFlag()
    {
        return flag;
    }

    public void SetFlag(bool value)
    {
        flag = value;
    }
}
