using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtScript : MonoBehaviour
{

    public Transform target;
    public bool Lookat;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Lookat)
        {
            Look();
        }
    }

    void Look()
    {
        CameraController.Instance.LookAtTarget = target;
        CameraController.Instance.BlockRotation = true;

    }
}
