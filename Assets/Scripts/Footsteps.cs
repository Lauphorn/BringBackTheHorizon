using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource LAudioS;
    public AudioSource RAudioS;

    public AudioClip Footstep;

    float Timer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
    }

    public void LeftStep()
    {
        if (Timer <= 0)
        {
            LAudioS.PlayOneShot(Footstep);
            Timer = 0.2f;
        }
    }

    public void RightStep()
    {
        if (Timer <= 0)
        {
            RAudioS.PlayOneShot(Footstep);
            Timer = 0.2f;
        }
    }
}
