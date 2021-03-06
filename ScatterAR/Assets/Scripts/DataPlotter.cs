using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using Vuforia;
using UnityEngine.Events;

public class DataPlotter : MonoBehaviour
{
    // Name of the input file, no extension
    public static string inputFile;

    public float plotScale = 10;

    // List for holding data from CSV reader
    private List<Dictionary<string, object>> pointList;

    // Indices for columns to be assigned
    public static int columnX;
    public static int columnY;
    public static int columnZ;

    string xName;
    string yName;
    string zName;

    // Number of separation on axis
    public int numParts=4;

    // Prefabs
    public GameObject pointPrefab;
    public GameObject axisPrefab;
    public TextMeshPro sampleText;

    // Object which will contain instantiated prefabs in hiearchy
    public GameObject pointHolder;

    // Axis Holders
    public GameObject fixedHolder;
    public GameObject[] axisHolder;

    public GameObject limiterCube;

    public TextMeshProUGUI[] axisNameText;

    // Axis color
    public Material[] axisMaterials;

    public GameObject Camera;

    public Button menuButton;

    // The 3 axis
    GameObject[] xyzaxis = new GameObject[3];

    // Max and Min values x,y,z
    float[] mMax = new float[3];
    float[] mMin = new float[3];

    int[] prevScale = new int[3];

    string[] tagList = { "X", "Y", "Z" };

    public static bool activator=false;


    // Use this for initialization
    void Start()
    {
        columnX = 0;
        columnY = 1;
        columnZ = 2;
        inputFile = "iris";
        //PlotScatter();
        
        menuButton.onClick.AddListener(EmptyPlot);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            EmptyPlot();
        if (Input.GetKeyDown(KeyCode.R))
            PlotScatter();

        if(activator == true)
        {
            activator = false;
            EmptyPlot();
            PlotScatter();
            SelectAxis.fillAxis = true;
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < 3; i++)
        {
            if(axisHolder[i].transform.parent.transform.localScale[i]/(Math.Pow(2,prevScale[i])) >= 1)
            {
                prevScale[i]++;
                List<Transform> childList = new List<Transform>();
                for (int j = 0; j < axisHolder[i].transform.childCount; j++)
                {
                    childList.Add(axisHolder[i].transform.GetChild(j));
                }

                childList = childList.OrderBy(e => e.localPosition[i]).ToList();

                for (int j = 0; j <childList.Count-1; j++)
                {
                    Vector3 posP = Vector3.zero;
                    posP[i] = (childList[j].localPosition[i] + childList[j+1].localPosition[i]) / 2;

                    float n1 = float.Parse(childList[j].name, System.Globalization.CultureInfo.InvariantCulture);
                    float n2 = float.Parse(childList[j+1].name, System.Globalization.CultureInfo.InvariantCulture);
                    float val = (n1 + n2) / 2;

                    GameObject pt = Instantiate(axisPrefab);
                    pt.transform.parent = axisHolder[i].transform;
                    pt.transform.localPosition = posP;
                    pt.transform.localRotation = Quaternion.identity;
                    pt.transform.localScale = childList[j].localScale;
                    pt.transform.name = val.ToString();
                    pt.tag = "Scale" + prevScale[i].ToString();
                    pt.GetComponent<Renderer>().material = axisMaterials[i];


                    TextMeshPro txt = Instantiate(sampleText);
                    txt.transform.parent = pt.transform;
                    txt.transform.localPosition = Vector3.zero;
                    txt.transform.localRotation = Quaternion.identity;
                    txt.transform.localScale = Vector3.one;
                    txt.text = val.ToString();
                }

                numParts *= 2;

            }

            if (axisHolder[i].transform.parent.transform.localScale[i] / (Math.Pow(2, prevScale[i]-1)) < 1 && prevScale[i] > 1)
            {
                prevScale[i]--;
                // Remove points
                List<Transform> childList = new List<Transform>();
                for (int j = 0; j < axisHolder[i].transform.childCount; j++)
                {
                    childList.Add(axisHolder[i].transform.GetChild(j));
                }

                for (int j = 0; j < childList.Count; j++)
                {
                    if (childList[j].tag == "Scale" + (prevScale[i] + 1).ToString())
                    {
                        Destroy(childList[j].gameObject);
                    }
                }
                
                numParts /= 2;
            }
        }
    }

    private float FindMaxValue(string columnName)
    {
        //set initial value to first value
        float maxValue = Convert.ToSingle(pointList[0][columnName]);

        //Loop through Dictionary, overwrite existing maxValue if new value is larger
        for (var i = 0; i < pointList.Count; i++)
        {
            if (maxValue < Convert.ToSingle(pointList[i][columnName]))
                maxValue = Convert.ToSingle(pointList[i][columnName]);
        }

        //Spit out the max value
        return maxValue;
    }

    private float FindMinValue(string columnName)
    {

        float minValue = Convert.ToSingle(pointList[0][columnName]);

        //Loop through Dictionary, overwrite existing minValue if new value is smaller
        for (var i = 0; i < pointList.Count; i++)
        {
            if (Convert.ToSingle(pointList[i][columnName]) < minValue)
                minValue = Convert.ToSingle(pointList[i][columnName]);
        }

        return minValue;
    }

    private void PlotScatter()
    {
        prevScale = new int[]{ 1, 1, 1 };
        numParts = 4;
        // Set pointlist to results of function Reader with argument inputfile
        pointList = CSVReader.Read(inputFile);

        //Log to console
        Debug.Log(pointList);

        // Declare list of strings, fill with keys (column names)
        List<string> columnList = new List<string>(pointList[1].Keys);

        // Print number of keys (using .count)
        Debug.Log("There are " + columnList.Count + " columns in CSV");

        foreach (string key in columnList)
            Debug.Log("Column name is " + key);

        // Assign column name from columnList to Name variables
        xName = columnList[columnX];
        yName = columnList[columnY];
        zName = columnList[columnZ];
        string[] mName = { xName, yName, zName };

        // Get maxes of each axis
        mMax[0] = FindMaxValue(xName);
        mMax[1] = FindMaxValue(yName);
        mMax[2] = FindMaxValue(zName);

        // Get minimums of each axis
        mMin[0] = FindMinValue(xName);
        mMin[1] = FindMinValue(yName);
        mMin[2] = FindMinValue(zName);


        pointHolder.transform.parent.transform.localPosition = (plotScale / 2) * Vector3.one;

        limiterCube.transform.localScale = plotScale*1.01f * Vector3.one;
        limiterCube.transform.localPosition = (plotScale*1.01f / 2) * Vector3.one;

        //Loop through Pointlist
        for (var i = 0; i < pointList.Count; i++)
        {
            // Get value in poinList at ith "row", in "column" Name, normalize
            float x =
            (System.Convert.ToSingle(pointList[i][xName]) - mMin[0]) / (mMax[0] - mMin[0]);

            float y =
            (System.Convert.ToSingle(pointList[i][yName]) - mMin[1]) / (mMax[1] - mMin[1]);

            float z =
            (System.Convert.ToSingle(pointList[i][zName]) - mMin[2]) / (mMax[2] - mMin[2]);

            //instantiate the prefab with coordinates defined above
            //Instantiate(pointPrefab, new Vector3(x, y, z), Quaternion.identity);

            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                    pointPrefab);

            // Make child of PointHolder object, to keep points within container in hierarchy
            dataPoint.transform.parent = pointHolder.transform;
            dataPoint.transform.localPosition = new Vector3(x, y, z) * plotScale - Vector3.one*(plotScale/2);
            dataPoint.transform.rotation = Quaternion.identity;

            // Assigns original values to dataPointName
            string dataPointName =
                pointList[i][xName] + " "
                + pointList[i][yName] + " "
                + pointList[i][zName];

            // Assigns name to the prefab
            dataPoint.transform.name = dataPointName;

            // Gets material color and sets it to a new RGBA color we define
            dataPoint.GetComponent<Renderer>().material.color =
            new Color(x, y, z, 1.0f);
        }

        // Draw the axis
        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = Vector3.zero;
            pos[i] = plotScale * 1.1f / 2;

            Vector3 sc = 0.002f * Vector3.one;
            sc[i] = plotScale * 1.1f;

            Vector3 colliderScale = 4 * Vector3.one;
            colliderScale[i] = 1;

            xyzaxis[i] = Instantiate(axisPrefab);
            xyzaxis[i].transform.parent = fixedHolder.transform;
            xyzaxis[i].transform.localPosition = pos;
            xyzaxis[i].transform.localRotation = Quaternion.identity;
            xyzaxis[i].transform.localScale = sc;
            xyzaxis[i].transform.name = mName[i];
            xyzaxis[i].GetComponent<Renderer>().material = axisMaterials[i];
            xyzaxis[i].GetComponent<BoxCollider>().size = colliderScale;
            xyzaxis[i].tag = tagList[i];

            axisNameText[i].text = mName[i];
            axisNameText[i].color = axisMaterials[i].color;

            Vector3 holderPos = Vector3.zero;
            holderPos[i] = plotScale / 2;
            axisHolder[i].transform.parent.transform.localPosition = holderPos;

            // Intermedial values
            float pSize = (mMax[i] - mMin[i]) / numParts;
            for (int j = 0; j < numParts + 1; j++)
            {
                Vector3 posP = Vector3.zero;
                posP[i] = (plotScale / numParts) * j - (plotScale / 2);
                GameObject pt = Instantiate(axisPrefab);
                pt.transform.parent = axisHolder[i].transform;
                pt.transform.localPosition = posP ;
                pt.transform.rotation = Quaternion.identity;
                pt.transform.localScale = 0.004f * Vector3.one;
                pt.transform.name = (pSize * j).ToString();
                pt.tag = "Scale1";
                pt.GetComponent<Renderer>().material = axisMaterials[i];

                Vector3 variation = -2*Vector3.one;
                variation[i] = 0;

                TextMeshPro txt = Instantiate(sampleText);
                txt.transform.parent = pt.transform;
                txt.transform.localPosition = variation;
                txt.transform.localRotation = Quaternion.identity;
                txt.transform.localScale = Vector3.one;
                txt.text = (pSize * j).ToString();
            }
        }
    }

    private void EmptyPlot()
    {
        foreach (Transform pt in pointHolder.transform)
            Destroy(pt.gameObject);
        for (int i=0; i < 3; i++)
        {
            foreach (Transform axisPt in axisHolder[i].transform)
                Destroy(axisPt.gameObject);
        }
        foreach (Transform axis in fixedHolder.transform)
            Destroy(axis.gameObject);
    }

}
