using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Test : MonoBehaviour
{
    Vector2 initialPosition;
    float totalDistance = 0;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 1 && SelectAxis.selectedAxis > -1 && SelectAxis.selectedAxis < 3) {
            var touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                totalDistance = 0;
                return;
            }

            if(touch.phase == TouchPhase.Began) {
                initialPosition = touch.position;
            } else {
                var distance = initialPosition.x - touch.position.x;

                totalDistance = distance;
                text.text = totalDistance.ToString();
            }
        }
    }

    bool FastApproximately(float a, int threshold)
    {
        return a >= -threshold && a <= threshold;
    }
}
