using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : MonoBehaviour
{
    public ChangeLight LightScript;
    public GameObject TVCanvas;
    bool Done;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Narration.Instance.Objects["Electricity"] == true && LightScript.On && !Done)
        {
            StartTv();
        }
        else
        {
            ShutDownTV();
        }
    }

    void StartTv()
    {
        TVCanvas.SetActive(true);
    }

    void ShutDownTV()
    {
        TVCanvas.SetActive(false);
    }
}
