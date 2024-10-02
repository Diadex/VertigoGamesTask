using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerWheelUIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] spinnerUIs;
    [SerializeField]
    private string[] spinnerUINames;

    [SerializeField]
    private string typeOfSpinner = "bronze";

    private void Start()
    {
        SetSpinnerWheelUI();
    }

    private void SetSpinnerWheelUI()
    {
        for (int i = 0; i < spinnerUIs.Length; i++)
        {
            if (spinnerUINames[i].Equals(typeOfSpinner))
                spinnerUIs[i].SetActive(true);
            else
                spinnerUIs[i].SetActive(false);
        }
    }

}
