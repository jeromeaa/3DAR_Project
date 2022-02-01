using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchToScale : MonoBehaviour
{
    // Pinch to Scale
    float initialDistance;
    Vector3 initialScale;
    Vector3 initialPtScale;

    // Slide to move
    Vector2 initialPosition;
    float totalDistance = 0;

    GameObject childMover;
    // Update is called once per frame

    private void Start()
    {
        childMover = transform.GetChild(0).gameObject;
    }
    void Update()
    {
        // Scale
        if (Input.touchCount == 2 && SelectAxis.selectedAxis > -1 && SelectAxis.selectedAxis < 3)
        {

            var touchZero = Input.GetTouch(0);
            var touchOne = Input.GetTouch(1);

            if (touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled ||
                touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
            {
                return;
            }

            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
                initialScale = this.transform.localScale;
                initialPtScale = childMover.transform.GetChild(0).transform.localScale;
            }
            else
            {
                var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);

                if (Mathf.Approximately(initialDistance, 0))
                {
                    return;
                }

                var factor = currentDistance / initialDistance;
                // Change the the code below to scale the right thing
                var scaled = initialScale;
                scaled[SelectAxis.selectedAxis] *= factor;
                if (scaled[SelectAxis.selectedAxis] > 1 && scaled[SelectAxis.selectedAxis]<=16)
                {
                    this.transform.localScale = scaled;

                    foreach (Transform child in childMover.transform)
                    {
                        var val = initialPtScale;
                        val[SelectAxis.selectedAxis] /= factor;
                        child.transform.localScale = val;
                    }
                }
            }

        }

        // Move
        //if (Input.touchCount == 1 && SelectAxis.selectedAxis > -1 && SelectAxis.selectedAxis < 3)
        //{
        //    var touch = Input.GetTouch(0);

        //    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        //    {
        //        totalDistance = 0;
        //        return;
        //    }

        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        initialPosition = touch.position;
        //    }
        //    else
        //    {
        //        var distance = initialPosition.x - touch.position.x;

        //        totalDistance = distance;
        //        text.text = totalDistance.ToString();
        //    }
        //}
    }
}
