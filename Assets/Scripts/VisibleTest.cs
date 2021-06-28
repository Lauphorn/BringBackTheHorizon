using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBecameInvisible()
    {
        Debug.Log("Invisible");
    }

    void OnBecameVisible()
    {
        Debug.Log("Visible");
    }
}
