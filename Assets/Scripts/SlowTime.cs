using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = 0.01f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;

        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;

        }
    }
}
