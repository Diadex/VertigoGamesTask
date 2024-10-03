using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinnerButtonHandler : MonoBehaviour
{
    [SerializeField] // The UI button
    private RectTransform spinButton;
    [SerializeField] // The UI button
    private GameObject spinButtonActiveGameObj;
    [SerializeField] // The UI button
    private GameObject spinButtonDisabledGameObj;
    // flag true when the button is pressed.
    private bool flag;
    private bool isActive = true;

    void Update()
    {
        if (isActive && Input.GetMouseButtonDown(0))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(spinButton, Input.mousePosition, null))
            {
                SetFlag(true);
                Debug.Log("Custom UI element clicked");
            }
        }
    }

    public void ButtonSetActive( bool isActiveVar)
    {
        spinButtonActiveGameObj.SetActive(isActiveVar);
        spinButtonDisabledGameObj.SetActive(!isActiveVar);
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
