using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] // The UI button RectTransform
    private RectTransform buttonRectTransform;
    [SerializeField] // The active state button GameObject
    private GameObject buttonActiveGameObj;
    [SerializeField] // The disabled button GameObject
    private GameObject buttonDisabledGameObj;
    [SerializeField] // The camera used in Screen Space - Camera mode
    private Camera uiCamera;

    // flag true when the button is pressed.
    private bool flag;
    private bool isActive = true;

    void Update()
    {
        if (isActive && Input.GetMouseButtonDown(0))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(buttonRectTransform, Input.mousePosition, uiCamera))
            {
                SetFlag(true);
                Debug.Log("Custom UI element clicked");
            }
        }
    }

    public void ButtonSetActive(bool isActiveVar)
    {
        buttonActiveGameObj.SetActive(isActiveVar);
        if (buttonDisabledGameObj != null)
        {
            buttonDisabledGameObj.SetActive(!isActiveVar);
        }
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
