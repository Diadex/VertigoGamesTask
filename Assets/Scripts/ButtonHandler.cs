using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] // The UI button
    private RectTransform buttonRectTransform;
    [SerializeField] // The UI button
    private GameObject buttonActiveGameObj;
    [SerializeField] // The UI button
    private GameObject buttonDisabledGameObj;
    // flag true when the button is pressed.
    private bool flag;
    private bool isActive = true;

    void Update()
    {
        if (isActive && Input.GetMouseButtonDown(0))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(buttonRectTransform, Input.mousePosition, null))
            {
                SetFlag(true);
                Debug.Log("Custom UI element clicked");
            }
        }
    }


    public void ButtonSetActive( bool isActiveVar)
    {
        buttonActiveGameObj.SetActive(isActiveVar);
        buttonDisabledGameObj.SetActive(!isActiveVar);
        isActive = isActiveVar;
    }

    // get and set for flag
    public bool GetFlag()
    {
        return flag;
    }

    public void SetFlag(bool value)
    {
        flag = value;
    }
}
