using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SelectAxis : MonoBehaviour
{
    public GameObject[] Axis;
    public Transform[] Buttons;

    public VirtualButtonBehaviour[] Vb;

    public static int selectedAxis = 1;

    bool buttonPressed = false;

    void Start()
    {
        Vb[0].RegisterOnButtonPressed(delegate { ButtonPress(0); });
        Vb[0].RegisterOnButtonReleased(delegate { ButtonRelease(0); });
        
        Vb[1].RegisterOnButtonPressed(delegate { ButtonPress(1); });
        Vb[1].RegisterOnButtonReleased(delegate { ButtonRelease(1); });
        
        Vb[2].RegisterOnButtonPressed(delegate { ButtonPress(2); });
        Vb[2].RegisterOnButtonReleased(delegate { ButtonRelease(2); });
        
        Vb[3].RegisterOnButtonPressed(delegate { ButtonPress(3); });
        Vb[3].RegisterOnButtonReleased(delegate { ButtonRelease(3); });
    }

    public void ButtonRelease(int a)
    {
        Buttons[a].localScale =  Vector3.one;
        buttonPressed = false;
    }

    public void ButtonPress(int a)
    {
        if (buttonPressed == false)
        {
            buttonPressed = true;
            Buttons[a].localScale = new Vector3(1, 0.2f, 1);

            if (a != selectedAxis)
            {
                if (selectedAxis != 3)
                {
                    Axis[selectedAxis].GetComponent<PinchToScale>().enabled = false;
                }
                // remove smtg from the physical axis

                if (a != 3)
                {
                    Axis[a].GetComponent<PinchToScale>().enabled = true;
                    // Do smtg with the physial axis
                }

                selectedAxis = a;
            }
        }
    }
}
