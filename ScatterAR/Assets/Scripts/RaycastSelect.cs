using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RaycastSelect : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI crosshair;

    private GameObject obj;
    private Vector3 scale;
    private bool transformed = false;

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            obj = hit.transform.gameObject;
            var tag = obj.tag;

            text.text = tag;

            if(tag == "Axis") {
                text.text = tag + " LOL";
                scale = obj.transform.localScale;
                obj.transform.parent.transform.localScale = scale * 2f;
                transformed = true;
            } else {
                if(transformed) {
                    obj.transform.localScale = scale;
                    transformed = false;
                }
            }
        }
    }
}
