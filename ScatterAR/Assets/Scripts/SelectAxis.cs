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
    Color[] savedColors = new Color[3];
    int hoveredAxis;

    public static bool fillAxis = false;

    void Start()
    {
        rayCastBtn.onClick.AddListener(ClickShot);
        for (int i = 0; i < 3; i++)
        {
            Axis[i] = fixedAxis.transform.GetChild(i).gameObject;
            savedColors[i] = mat[i].color;
        }

    }

    private void Update()
    {
        ShootRay(true);

        if (fillAxis)
        {
            fillAxis = false;
            fillAxisArray();
        }
    }

    void fillAxisArray()
    {
        for (int i = 0; i < 3; i++)
        {
            Axis[i] = fixedAxis.transform.GetChild(i).gameObject;
        }
    }
    public void ChangeAxis(int a, bool hover)
    {
        int prev;
        if (hover)
            prev = hoveredAxis;
        else prev = selectedAxis;

        if (a != prev)
        {
            if (prev != 3)
            {
                if (!hover)
                {
                    AxisPoints[prev].GetComponent<PinchToScale>().enabled = false;
                    mat[prev].color = savedColors[prev];
                }

                if (hoveredAxis != selectedAxis)
                {
                    Axis[prev].GetComponent<Outline>().enabled = false;
                    for (int i = 0; i < AxisPoints[prev].transform.GetChild(0).childCount; i++)
                    {
                        AxisPoints[prev].transform.GetChild(0).transform.GetChild(i).GetComponent<Outline>().enabled = false;
                    }
                }

            }

            if (a != 3)
            {
                if (!hover)
                {
                AxisPoints[a].GetComponent<PinchToScale>().enabled = true;
                mat[a].color = mat[3].color;
                }

                Axis[a].GetComponent<Outline>().enabled = true;

                for (int i = 0; i < AxisPoints[a].transform.GetChild(0).childCount; i++)
                {
                    AxisPoints[a].transform.GetChild(0).transform.GetChild(i).GetComponent<Outline>().enabled = true;
                }

            }
            if (hover)
                hoveredAxis = a;
            else selectedAxis = a;
        }
    }

    void ClickShot()
    {
        ShootRay(false);
    }

    private void ShootRay(bool hover)
    {
        Ray ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity,~(LayerMask.GetMask("Limiter")|LayerMask.GetMask("Point"))))
        {
            var tag = hit.transform.gameObject.tag;
            Debug.Log(tag);

            var val = Array.IndexOf(tagList, tag);
            Debug.Log(val);
            if (val !=-1)
            {
                ChangeAxis(val,hover);
            }
            else
            {
                ChangeAxis(3,hover);
            }
        }
        else
        {
            ChangeAxis(3,hover);
        }
    }

}
