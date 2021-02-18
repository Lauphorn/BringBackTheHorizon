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
    }
}
