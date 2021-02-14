using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource LAudioS;
    public AudioSource RAudioS;



    public List<AudioClip> Footstep;

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
            LAudioS.PlayOneShot(Footstep[Random.Range(0,Footstep.Capacity)]);
            Timer = 0.2f;
        }
    }

    public void RightStep()
    {
        if (Timer <= 0)
        {
            RAudioS.PlayOneShot(Footstep[Random.Range(0, Footstep.Capacity)]);
            Timer = 0.2f;
        }
    }
}
