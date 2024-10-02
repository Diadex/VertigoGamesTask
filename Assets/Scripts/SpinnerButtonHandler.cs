using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinnerButtonHandler : MonoBehaviour
{
    [SerializeField] // The UI button
    private RectTransform spinButton;

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
                Debug.Log("Custom UI element clicked");
            }
        }
    }
}
