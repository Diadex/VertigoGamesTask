using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinnerButtonHandler : MonoBehaviour
{
    [SerializeField] // The UI button
    private RectTransform spinButton;
    private bool flag;

    void Start()
    {
        spinButton = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(spinButton, Input.mousePosition, null))
            {
                SetFlag(true);
                Debug.Log("Custom UI element clicked");
            }
        }
    }

    // get and set for flag
    public bool IsFlagSet
    {
        get { return flag; }
    }

    public void SetFlag(bool value)
    {
        flag = value;
    }
}
