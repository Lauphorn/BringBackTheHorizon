using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    public Animator KnobAnimator;
    public bool InRange, NarrationActivationCheck, Grabbed;

    public Narration.Narrations NarrationNeeded;
    bool NarrationNeededCheck;

    public enum LogoChoice
    {
        Hand,
        Eye
    }
    public LogoChoice logo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (KnobAnimator != null)
        {
            if (InRange && NarrationActivationCheck)
            {
                KnobAnimator.SetBool("Ball", true);
            }
            else
            {
                KnobAnimator.SetBool("Ball", false);
            }

            if (logo == LogoChoice.Eye && looked)
            {
                KnobAnimator.SetBool("Eye", true);
            }
            else
            {
                KnobAnimator.SetBool("Eye", false);
            }

            if (logo == LogoChoice.Hand && looked)
            {
                KnobAnimator.SetBool("Hand", true);
            }
            else
            {
                KnobAnimator.SetBool("Hand", false);
            }

            if (interactable)
            {
                KnobAnimator.SetBool("Interactable", true);
            }
            else
            {
                KnobAnimator.SetBool("Interactable", false);
            }

            if (blocked)
            {
                KnobAnimator.SetBool("Locked", true);
            }
            else
            {
                KnobAnimator.SetBool("Locked", false);
            }
        }*/

    }

    public void Grab()
    {

    }

    public void Release()
    {

    }
}
