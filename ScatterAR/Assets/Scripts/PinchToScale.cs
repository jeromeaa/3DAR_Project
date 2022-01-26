using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Select : MonoBehaviour
{
    private float initialDistance;
    private Vector3 initialScale;
    // The game object which will be scaled
    public GameObject gameObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 2) {
            var touchZero = Input.GetTouch(0);
            var touchOne = Input.GetTouch(1);

            if(touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled ||
                touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled) {
                    return;
            }

            if(touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began) {
                initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
                initialScale = gameObject.transform.localScale;
                Debug.Log("Initial distance: " + initialDistance + "GameObject name: " + gameObject.name);
            } else {
                var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);

                if(Mathf.Approximately(initialDistance, 0)) {
                    return;
                }

                var factor = currentDistance / initialDistance;
                // Change the the code below to scale the right thing
                var scaled = initialScale.x * factor;
                gameObject.transform.localScale = new Vector3(scaled, initialScale.y, initialScale.z);
            }

        }
        
    }
}
