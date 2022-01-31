using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using TMPro;
using System;

public class SelectAxis : MonoBehaviour
{
    public GameObject[] Axis;
    public Material[] mat;
    public TextMeshProUGUI crosshair;

    public TextMeshProUGUI textExample;

    public static int selectedAxis = 3;

    public Button rayCastBtn;
    
    string[] tagList=  {"X","Y","Z"};
    Color savedColor;

    void Start()
    {
        rayCastBtn.onClick.AddListener(ShootRay);
    }


    public void ChangeAxis(int a)
    {
        if (a != selectedAxis)
        {
            if (selectedAxis != 3)
            {
                Axis[selectedAxis].GetComponent<PinchToScale>().enabled = false;
                mat[selectedAxis].color = savedColor;
            }

            if (a != 3)
            {
                Axis[a].GetComponent<PinchToScale>().enabled = true;
                savedColor = mat[a].color;
                mat[a].color = mat[3].color;
            }

            selectedAxis = a;
        }
    }

    private void ShootRay()
    {
        Debug.Log("SHOOOOOT");
        Ray ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            var tag = hit.transform.gameObject.tag;
            textExample.text = tag;

            var val = Array.IndexOf(tagList, tag);
            Debug.Log(val);
            if (val !=-1)
            {
                ChangeAxis(val);
            }
            else
            {
                ChangeAxis(3);
            }
        }
        else
        {
            ChangeAxis(3);
        }
    }

}
