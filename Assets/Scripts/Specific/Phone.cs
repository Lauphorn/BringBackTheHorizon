using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{

    public List<Narration.Narrations> ListeDeNarrations;
    public List<SousTitre> ListeDeVoix;

    int NuméroVoix;

    public GameObject Light;
    float LightTimer;
    bool NarrationCheckDone, OnLight, Next, start;


    public InteractableObject Anim;
    AudioSource Audio;
    Narration narr;
    Voice voix;


    void Start()
    {
        narr = Narration.Instance;
        voix = Voice.Instance;
        Audio = gameObject.GetComponent<AudioSource>();
        LightTimer = 1;
        Next = true;

        Debug.Log(ListeDeVoix.Count);
    }

    // Update is called once per frame
    void Update()
    {

        if (!NarrationCheckDone)
        {
            if (narr.CheckNarration(Narration.Narrations.Electricity))
            {
                OnLight = true;
                NarrationCheckDone = true;
            }

        }

        if (OnLight)
        {
            if (LightTimer <= 0)
            {
                LightTimer = 0.5f;
                Light.SetActive(!Light.activeInHierarchy);
            }
            else
            {
                LightTimer -= Time.deltaTime;
            }
        }

        if (start)
        {
            if (NarrationCheckDone)
            {
                if (Next)
                {
                    OnLight = true;

                    if (!voix.IsPlaying)
                    {
                        if (NuméroVoix < ListeDeVoix.Count)
                        {
                            Launch();
                            Next = false;
                        }
                        else
                        {
                            Anim.Release();
                            start = false;
                        }
                    }
                }
            }
        }
        
    }

    public void Launch()
    {
        StartCoroutine(LaunchMessage());
        start = true;
    }

    IEnumerator LaunchMessage()
    {
        yield return new WaitForSeconds(0.5f);
        ListeDeVoix[NuméroVoix].Talk();
        NuméroVoix += 1;
        Next = true;
    }



}
