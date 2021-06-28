using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    public Animator KnobAnimator;
    bool InRange, NarrationActivationCheck, Grabbed, narrCheck;
    [HideInInspector]
    public bool looked, interactable;
    public Narration.Narrations NarrationNeeded;

    GameObject CameraPlace;
    GameObject GrabFollowPosition;

    public bool ShowIfNarration;

    public enum LogoChoice
    {
        Hand,
        Eye
    }
    public LogoChoice logo;

    // Start is called before the first frame update
    void Start()
    {
        CameraPlace = CameraController.Instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
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

            if (logo == LogoChoice.Hand && looked)
            {
                KnobAnimator.SetBool("Hand", true);
            }
            else
            {
                KnobAnimator.SetBool("Hand", false);
            }

            if (looked)
            {
                KnobAnimator.SetBool("Interactable", true);
            }
            else
            {
                KnobAnimator.SetBool("Interactable", false);
            }
        }

        if (Grabbed)
        {
            transform.GetComponent<Rigidbody>().AddForce((GrabFollowPosition.transform.position - transform.position)*0.75f,ForceMode.VelocityChange);
            transform.GetComponent<Rigidbody>().drag = 5 - (Vector3.Magnitude(GrabFollowPosition.transform.position - transform.position))*2;
            Debug.Log("magnitude  " + Vector3.Magnitude(GrabFollowPosition.transform.position - transform.position));
        }

        if (ShowIfNarration)
        {
            if (Narration.Instance.NarrationHasChanged != narrCheck)
            {
                narrCheck = Narration.Instance.NarrationHasChanged;

                if (Narration.Instance.CheckNarration(NarrationNeeded))
                {
                    NarrationActivationCheck = true;
                }
                else
                {
                    NarrationActivationCheck = false;
                }
            }
        }
        else
        {
            NarrationActivationCheck = true;
        }
    }

    public void Grab()
    {
        if (NarrationActivationCheck)
        {
            Debug.Log("Grabbed");
            KnobAnimator.SetBool("Grabbed", true);
            transform.GetComponent<Rigidbody>().useGravity = false;
            Grabbed = true;
            GrabFollowPosition = CameraPlace.transform.GetChild(1).gameObject;
        }
    }

    public void Release()
    {
        Debug.Log("Released");
        KnobAnimator.SetBool("Grabbed", false);
        transform.GetComponent<Rigidbody>().useGravity = true;
        transform.GetComponent<Rigidbody>().drag = 0;
        Grabbed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ItemRange")
        {
            InRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ItemRange")
        {
            InRange = false;
        }
    }
}
