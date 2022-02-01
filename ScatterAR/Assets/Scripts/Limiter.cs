using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limiter : MonoBehaviour
{
      private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Limit")
        {
            this.GetComponent<Renderer>().enabled = false;

            if (transform.childCount>0)
                transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Limit" && GetComponent<Renderer>().enabled == false)
        {
            this.GetComponent<Renderer>().enabled = true;

            if (transform.childCount>0)
                transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = true;
        }
    }
}
