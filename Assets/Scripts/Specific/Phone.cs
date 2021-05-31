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
    bool NarrationCheckDone, OnLight, Next;


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
                Anim.ParentActivated = true;
            }
            else
            {
                Anim.ParentActivated = false;
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

        if (NarrationCheckDone)
        {
            if (Next)
            {
                OnLight = true;

                if (voix.IsPlaying)
                {
                    Anim.ParentActivated = false;
                }
                else
                {
                    Anim.ParentActivated = true;
                }
            }
            else
            {
                OnLight = false;
                Anim.interactable = false;
            }


        }
    }

    public void Launch()
    {
        Audio.Play();
        ListeDeVoix[NuméroVoix].Talk();
        NuméroVoix += 1;
        if(NuméroVoix < ListeDeVoix.Count)
        {
            if (narr.CheckNarration(ListeDeNarrations[NuméroVoix]))
            {
                Next = true;
            }
            else
            {
                Next = false;
            }
        }
        else
        {
            Next = false;
        }

    }
}
