using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using TMPro;
using System;

public class SelectAxis : MonoBehaviour
{
    public GameObject[] AxisPoints;
    public GameObject fixedAxis;
    GameObject[] Axis = new GameObject[3];
    public Material[] mat;
    public TextMeshProUGUI crosshair;

    public static int selectedAxis = 3;

    public Button rayCastBtn;
    
    string[] tagList=  {"X","Y","Z"};
    Color savedColor;

    void Start()
    {
        rayCastBtn.onClick.AddListener(ShootRay);
        for (int i = 0; i < 3; i++)
        {
            Axis[i] = fixedAxis.transform.GetChild(i).gameObject;
        }

    }



    public void ChangeAxis(int a)
    {
        if (a != selectedAxis)
        {
            if (selectedAxis != 3)
            {
                AxisPoints[selectedAxis].GetComponent<PinchToScale>().enabled = false;
                Axis[selectedAxis].GetComponent<Outline>().enabled = false;

                mat[selectedAxis].color = savedColor;
            }

            if (a != 3)
            {
                AxisPoints[a].GetComponent<PinchToScale>().enabled = true;
                Axis[a].GetComponent<Outline>().enabled = true;
                savedColor = mat[a].color;
                mat[a].color = mat[3].color;
            }

            selectedAxis = a;
        }
    }

    private void ShootRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity,~LayerMask.GetMask("Limiter")))
        {
            var tag = hit.transform.gameObject.tag;
            Debug.Log(tag);

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
