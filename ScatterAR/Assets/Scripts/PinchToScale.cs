using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchToScale : MonoBehaviour
{
    float initialDistance;
    Vector3 initialScale;
    Vector3 initialPtScale;

    // Update is called once per frame
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
                initialPtScale = this.transform.GetChild(0).transform.localScale;
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
                if (scaled[SelectAxis.selectedAxis] > 1)
                {
                    this.transform.localScale = scaled;

                    foreach (Transform child in this.transform)
                    {
                        var val = initialPtScale;
                        val[SelectAxis.selectedAxis] /= factor;
                        child.transform.localScale = val;
                    }
                }
            }

        }

    }
}
