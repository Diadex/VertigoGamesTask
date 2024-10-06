using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// manages on which round the wheel looks as how. Whether it is gold, silver, bronze etc.
public class SpinnerWheelUIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] spinnerUIs;
    [SerializeField]
    private GameObject[] spinnerUIWheelParents;
    [SerializeField]
    private string[] spinnerUINames;
    [SerializeField]
    private GameObject slotsOfWheelGameObj;

    private string typeOfSpinner = "bronze";


    private void SetSpinnerWheelUI()
    {
        for (int i = 0; i < spinnerUIs.Length; i++)
        {
            if (spinnerUINames[i].Equals(typeOfSpinner))
            {
                spinnerUIs[i].SetActive(true);
                slotsOfWheelGameObj.transform.SetParent(spinnerUIWheelParents[i].transform);
            }
            else
            {
                spinnerUIs[i].SetActive(false);
            }
        }
    }


    public string SpinnerInitialise( int round)
    {
        string spinnertype;
        if (spinnerUINames.Length < 3)
        {
            Debug.Log("Error: There are less types of spinners than the types of variations. SpinnerWheelUIManager");
            return "Error";
        }
        // gold
        if (round % 30 == 0)
        {
            spinnertype = spinnerUINames[2];
        }// silver
        else if (round % 5 == 0)
        {
            spinnertype = spinnerUINames[1];
        }// bronze
        else
        {
            spinnertype = spinnerUINames[0];
        }
        typeOfSpinner = spinnertype;
        SetSpinnerWheelUI();
        return spinnertype;
    }

    public GameObject[] GetSpinnerWheelUIReferences()
    {
        return spinnerUIWheelParents;
    }

}
