using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{

    public Camera mCam;
    int layer_mask;
    InteractableObject savedObject;
    GrabObject savedGrab;

    bool grabbed;


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

        if (Physics.Raycast(mCam.transform.position, mCam.transform.forward, out hit, 10.0f, layer_mask) && hit.collider.tag == "Item")
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

            if (hit.transform.GetComponent<InteractableObject>() != null)
            {
                if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !FpsController.Instance.InAnim)
                {
                    hit.transform.GetComponent<InteractableObject>().Interacted();
                }
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


        Debug.DrawRay(mCam.transform.position, mCam.transform.forward, Color.green);
        RaycastHit hit2;

        if (Physics.Raycast(mCam.transform.position, mCam.transform.forward, out hit2, 1.5f, layer_mask) && hit.collider.tag == "Item")
        {
            if (savedGrab != hit.transform.GetComponent<GrabObject>())
            {
                if (savedGrab != null)
                {
                    savedGrab.looked = false;
                }

                savedGrab = hit.transform.GetComponent<GrabObject>();
                savedGrab.looked = true;
                savedGrab.interactable = true;
            }

            if (hit.transform.GetComponent<GrabObject>() != null)
            {
                if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !FpsController.Instance.InAnim)
                {
                    hit.transform.GetComponent<GrabObject>().Grab();
                    grabbed = true;
                }
            }

        }
        else
        {
            if (savedGrab != null && !grabbed)
            {
                savedGrab.looked = false;
                savedGrab.interactable = false;

                savedGrab = null;
            }
        }
        if (grabbed)
        {
            if ((Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space)) && !FpsController.Instance.InAnim)
            {
                savedGrab.Release();
                grabbed = false;
            }
        }

    }

}
