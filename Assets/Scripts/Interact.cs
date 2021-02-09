using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{

    public Camera mCam;
    int layer_mask;
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

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.Raycast(mCam.transform.position, mCam.transform.forward, out hit, 100.0f,layer_mask) && hit.collider.tag == "Item")
            {
                Debug.Log("Hit Printing Press");
                Debug.Log(hit.transform.gameObject.name);
                hit.transform.GetComponent<IkObject>().Interacted();
            }
        }

    }
}
