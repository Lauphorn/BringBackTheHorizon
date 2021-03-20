using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowCam : MonoBehaviour
{
    Transform Cam;
    void Start()
    {
        Cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Cam.rotation;
        transform.localScale = new Vector3(Vector3.Distance(transform.position, Cam.position)*1.5f, Vector3.Distance(transform.position, Cam.position)*1.5f,1);
    }
}
