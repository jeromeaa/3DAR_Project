using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Vuforia;
using UnityEngine.UI;
using TMPro;

public class ChangeText : MonoBehaviour
{
    // Start is called before the first frame update
    string inputFil="iris";
    private List<Dictionary<string, object>> pointList;

    public VirtualButtonBehaviour VBTN_X;
    public VirtualButtonBehaviour VBTN_Y;
    public VirtualButtonBehaviour VBTN_Z;
    public VirtualButtonBehaviour VBTN_data;

    public VirtualButtonBehaviour VBTN_Confirm;

    public TextMeshProUGUI message_X;
    public TextMeshProUGUI message_Y;
    public TextMeshProUGUI message_Z;
    public TextMeshProUGUI message_data;

    public GameObject menu;
    public GameObject ui;

    int nX=0;
    int nY=1;
    int nZ=2;
    int ndata=0;

    List<string> columnList = new List<string>();
    List<string> dataset_list = new List<string>();
    string dataset_Path = @"..\ScatterAR\Assets\Resources\";
    
    void Start()
    {
        //plotter.SetActive(false);
        ui.SetActive(false);

        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dataset_Path);
        
        foreach (var file in di.GetFiles("*.csv"))
        {
            dataset_list.Add(System.IO.Path.GetFileNameWithoutExtension(file.Name));
        }
        //Default dataset
        pointList = CSVReader.Read(inputFil);
        columnList = new List<string>(pointList[1].Keys);


        message_X.text = columnList[nX];
        message_Y.text = columnList[nY];
        message_Z.text = columnList[nZ];

        VBTN_Confirm.RegisterOnButtonPressed(F_Confirm);
        VBTN_X.RegisterOnButtonPressed(X_next_string);
        VBTN_Y.RegisterOnButtonPressed(Y_next_string);
        VBTN_Z.RegisterOnButtonPressed(Z_next_string);
        VBTN_data.RegisterOnButtonPressed(data_next_string);
    }

    public void X_next_string(VirtualButtonBehaviour vb)
    {
        message_X.text = columnList[nX % columnList.Count];
        nX = nX + 1;
        
    }
    public void Y_next_string(VirtualButtonBehaviour vb)
    {
        message_Y.text = columnList[nY % columnList.Count];
        nY = nY + 1;
    }
    public void Z_next_string(VirtualButtonBehaviour vb)
    {
        message_Z.text = columnList[nZ % columnList.Count];
        nZ = nZ + 1;
    }
    public void data_next_string(VirtualButtonBehaviour vb)
    {
        message_data.text = dataset_list[ndata % dataset_list.Count];

        ndata = ndata + 1;
        inputFil = message_data.text;
        pointList = CSVReader.Read(inputFil);
        columnList = new List<string>(pointList[1].Keys);
        nX = 0;
        nY = 1;
        nZ = 2;
        message_X.text = columnList[nX];
        message_Y.text = columnList[nY];
        message_Z.text = columnList[nZ];
    }
    public void F_Confirm(VirtualButtonBehaviour vb)
    {
        Debug.Log(nX + "_" + nY + "_" + nZ + "__" + inputFil);
        VBTN_Confirm.enabled = false;
        VBTN_X.enabled = false;
        VBTN_Y.enabled = false;
        VBTN_Z.enabled = false;
        VBTN_data.enabled = false;

        DataPlotter.columnX = nX;
        DataPlotter.columnY = nY;
        DataPlotter.columnZ = nZ;

        DataPlotter.inputFile = inputFil;

        DataPlotter.activator = true;

        ui.SetActive(true);
        menu.SetActive(false);
    }

}





