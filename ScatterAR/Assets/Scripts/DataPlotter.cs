using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class DataPlotter : MonoBehaviour
{
    // Name of the input file, no extension
    public string inputFile;

    public float plotScale = 10;

    // List for holding data from CSV reader
    private List<Dictionary<string, object>> pointList;

    // Indices for columns to be assigned
    public int columnX = 0;
    public int columnY = 1;
    public int columnZ = 2;

    // Full column names
    public string xName;
    public string yName;
    public string zName;

    // Number of separation on axis
    public int numParts=5;

    // The prefab for the data points that will be instantiated
    public GameObject pointPrefab;

    // Object which will contain instantiated prefabs in hiearchy
    public GameObject pointHolder;

    // The Object of the axis
    public GameObject axis;

    // Text object
    public TextMeshPro sampleText;

    // Axis text
    public TextMeshProUGUI[] axisNameText;

    // Axis color
    public Color[] axisColor;

    // The Camera
    public GameObject Camera;

    // The 3 axis
    GameObject[] xyzaxis = new GameObject[3];

    // Use this for initialization
    void Start()
    {

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
        float xMax = FindMaxValue(xName);
        float yMax = FindMaxValue(yName);
        float zMax = FindMaxValue(zName);
        float[] mMax = { xMax, yMax, zMax };

        // Get minimums of each axis
        float xMin = FindMinValue(xName);
        float yMin = FindMinValue(yName);
        float zMin = FindMinValue(zName);
        float[] mMin = { xMin, yMin, zMin };

        //Loop through Pointlist
        for (var i = 0; i < pointList.Count; i++)
        {
            // Get value in poinList at ith "row", in "column" Name, normalize
            float x =
            (System.Convert.ToSingle(pointList[i][xName]) - xMin) / (xMax - xMin);

            float y =
            (System.Convert.ToSingle(pointList[i][yName]) - yMin) / (yMax - yMin);

            float z =
            (System.Convert.ToSingle(pointList[i][zName]) - zMin) / (zMax - zMin);

            //instantiate the prefab with coordinates defined above
            //Instantiate(pointPrefab, new Vector3(x, y, z), Quaternion.identity);

            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                    pointPrefab,
                    new Vector3(x, y, z)*plotScale,
                    Quaternion.identity);

            // Make child of PointHolder object, to keep points within container in hierarchy
            dataPoint.transform.parent = pointHolder.transform;

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

            Vector3 sc = 0.001f * Vector3.one;
            sc[i] = plotScale * 1.1f;

            xyzaxis[i] = Instantiate(axis, pos, Quaternion.identity);
            xyzaxis[i].transform.localScale = sc;
            xyzaxis[i].transform.parent = pointHolder.transform;
            xyzaxis[i].transform.name = mName[i];
            xyzaxis[i].GetComponent<Renderer>().material.color = axisColor[i];
            
            axisNameText[i].text = mName[i];
            axisNameText[i].color = axisColor[i];

            // Intermedial values
            float pSize = (mMax[i] - mMin[i])/numParts;
            for (int j = 1; j < numParts+1; j++)
            {
                Vector3 posP = Vector3.zero;
                posP[i] = (plotScale / numParts) * j;
                GameObject pt = Instantiate(axis, posP, Quaternion.identity);
                pt.transform.localScale = 0.002f * Vector3.one;
                pt.transform.parent = pointHolder.transform;
                pt.GetComponent<Renderer>().material.color = axisColor[i];

                TextMeshPro txt = Instantiate(sampleText, posP, Quaternion.identity);
                txt.text = (pSize * j).ToString();
                txt.transform.localScale = 0.002f * Vector3.one;
                txt.transform.parent = pointHolder.transform;
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
}
