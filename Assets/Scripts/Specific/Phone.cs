using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{

    public List<Narration.Narrations> ListeDeNarrations;
    public List<SousTitre> ListeDeVoix;

    int NuméroVoix, messagesDispo;

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
            else
            {
                Anim.done = true;
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
                    Anim.done = true;
                }
                else
                {
                    Anim.done = false;
                }
            }
            else
            {
                OnLight = false;
                Anim.done = false;
            }


        }
    }

    public void Launch()
    {
        Audio.Play();
        ListeDeVoix[NuméroVoix].Talk();

        while(Next)
        {
            if (NuméroVoix < ListeDeVoix.Count)
            {
                if (ListeDeVoix[NuméroVoix] != null)
                {
                    if (narr.CheckNarration(ListeDeNarrations[NuméroVoix]))
                    {
                        Next = true;
                        messagesDispo += 1;
                    }
                    else
                    {
                        Next = false;
                    }
                }

            }
            else
            {
                Next = false;
            }
        }
    }

}
