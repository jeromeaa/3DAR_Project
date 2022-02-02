using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Vuforia;

public class ButtonInterface : MonoBehaviour
{
    public Button menuButton;
    public Button resetButton;
    public Button crossButton;
    public Button selectButton;

    public VirtualButtonBehaviour[] vbtn;

    public TextMeshProUGUI cross;

    public Transform[] scalers;

    //Cross
    public SelectAxis selectScript;

    public GameObject menu;
    public GameObject ui;

    void Start()
    {
        menuButton.onClick.AddListener(ShowHideMenu);
        resetButton.onClick.AddListener(ResetPlot);
        crossButton.onClick.AddListener(OnOffCross);

        selectButton.interactable = false;
        cross.enabled = false;
        selectScript.enabled = false;
    }

    void ResetPlot()
    {
        foreach(Transform sc in scalers)
        {
            float scale;
            if (sc.name == "Points")
                scale = 0.01f;
            else scale = 0.004f;
            sc.localScale = Vector3.one;
            sc.GetChild(0).localPosition = Vector3.zero;
            foreach(Transform child in sc.GetChild(0))
            {
                child.localScale = scale*Vector3.one;
            }
        }
    }

    void OnOffCross()
    {
        if (selectScript.enabled)
        {
            selectScript.ChangeAxis(3, false);
            selectScript.ChangeAxis(3, true);
        }
        selectScript.enabled = !selectScript.enabled;
        selectButton.interactable = !selectButton.interactable;
        //resetButton.interactable = !resetButton.interactable;
        cross.enabled = !cross.enabled;
    }

    void ShowHideMenu()
    {
        if (selectScript.enabled)
            OnOffCross();
        for (int i = 0; i < 5; i++)
            vbtn[i].enabled = true;

        menu.SetActive(true);
        ui.SetActive(false);
    }


}
