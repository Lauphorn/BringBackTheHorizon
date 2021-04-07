using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{

    public Camera mCam;
    int layer_mask;
    InteractableObject savedObject;

    // Start is called before the first frame update
    void Start()
    {
        layer_mask = LayerMask.GetMask("Item");
    }

    // Update is called once per frame
    void Update()
    {

        Debug.DrawRay(mCam.transform.position, mCam.transform.forward, Color.red);
        RaycastHit hit;

        if (Physics.Raycast(mCam.transform.position, mCam.transform.forward, out hit, 100.0f, layer_mask) && hit.collider.tag == "Item")
        {
            if(savedObject != hit.transform.GetComponent<InteractableObject>())
            {
                if(savedObject != null)
                {
                    savedObject.looked = false;
                }

                savedObject = hit.transform.GetComponent<InteractableObject>();
                savedObject.looked = true;
            }


            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !FpsController.Instance.InAnim)
            {
                Debug.Log("Hit Printing Press");
                hit.transform.GetComponent<InteractableObject>().Interacted();
            }
        }
        else
        {
            if(savedObject != null)
            {
                savedObject.looked = false;
                savedObject = null;
            }
        }
    }
}
