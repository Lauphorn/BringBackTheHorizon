using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.Events;
public class IkObject : MonoBehaviour
{

    FpsController bodyController;
    CameraController cameraController;
    IkHand Handcontroller;

    public bool OneUse;

    public bool MoveBody;
    [ConditionalField("MoveBody")] public Transform BodyFollowPosition;

    public bool Crouch;
    [ConditionalField("Crouch")] public float CrouchWeight;

    public bool LookAtTarget;
    [ConditionalField("LookAtTarget")] public Transform LookFollowPosition;

    public bool LaunchAnim;
    [ConditionalField("LaunchAnim")] public string AnimBool;

    public bool LaunchFunction;
    [ConditionalField("LaunchFunction")] public UnityEvent OtherFunctions;

    public bool ChangeNarration;
    [ConditionalField("ChangeNarration")] public string NarrationChangedString;

    public bool NeedNarration;
    [ConditionalField("NeedNarration")] public string NarrationNeededString;
    bool NarrationNeededCheck;

    public bool UseVoice;
    public List<AudioClip> VoiceClip;
    public List<string> VoiceLine;
    [ConditionalField("UseVoice")] public int VoiceNumber;


    public bool MoveHands;
    [ConditionalField("MoveHands")] public bool MoveRightHand;
    [ConditionalField("MoveRightHand")] public Transform RightHandFollowPosition;
    [ConditionalField("MoveRightHand")] public HandLineRenderer RightHand;
    [ConditionalField("MoveRightHand")] public float RightHandWeight;


    [ConditionalField("MoveHands")] public bool MoveLefttHand;
    [ConditionalField("MoveLefttHand")] public Transform LeftHandFollowPosition;
    [ConditionalField("MoveLefttHand")] public HandLineRenderer LeftHand;
    [ConditionalField("MoveLefttHand")] public float LeftHandWeight;

    public enum LogoChoice
    {
        Hand,
        Eye
    }
    public LogoChoice logo;

    public Animator Anim;
    public Animator KnobAnimator;
    bool InRange, interacted, done;
    [HideInInspector]
    public bool interactable,looked;
    bool AnimBlock;

    // Start is called before the first frame update
    void Start()
    {
        bodyController = FpsController.Instance;
        cameraController = CameraController.Instance;
        Handcontroller = IkHand.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!interacted && KnobAnimator.GetBool("Ball") == false)
        {
            KnobAnimator.SetBool("Ball", (InRange && !bodyController.InAnim && done !=true));
        }
        else
        {
            KnobAnimator.SetBool("Ball", false);
        }
 
        if (logo == LogoChoice.Eye && KnobAnimator.GetBool("Eye") == false)
        {
            KnobAnimator.SetBool("Eye", looked);
        }
        else
        {
            KnobAnimator.SetBool("Eye", false);
        }

        if (logo == LogoChoice.Hand && KnobAnimator.GetBool("Hand") == false)
        {
            KnobAnimator.SetBool("Hand", looked);
        }
        else
        {
            KnobAnimator.SetBool("Hand", false);
        }

        if (interactable && KnobAnimator.GetBool("Interactable") == false)
        {
            KnobAnimator.SetBool("Interactable", true);
        }
        else
        {
            KnobAnimator.SetBool("Interactable", false);
        }
        */


        if (interacted)
        {

            bodyController.InAnim=true;

            if (Crouch)
            {
                Handcontroller.CrouchWeight = CrouchWeight;
            }

            if (MoveBody)
            {
                bodyController.BlockMove = true;
                bodyController.BodyFollowPosition = BodyFollowPosition;
            }

            if (MoveHands)
            {
                if (MoveLefttHand)
                {
                    Handcontroller.LeftHandWeight = LeftHandWeight;
                }
                if (MoveRightHand)
                {
                    Handcontroller.RightHandWeight = RightHandWeight;
                }
            }
            
            if (!AnimBlock)
            {

                if (bodyController.MoveDone == true || !MoveBody)
                {
                    if (NeedNarration)
                    {
                        NarrationNeededCheck = Narration.Instance.Objects[NarrationNeededString];
                        Anim.SetBool("NarrationNeeded", NarrationNeededCheck);
                    }
                    Anim.SetTrigger("Interact");
                    if (LaunchAnim)
                    {

                        Handcontroller.LaunchAnim(AnimBool);
                    }
                    AnimBlock = true;
                }

                if (LookAtTarget && (bodyController.MoveDone == true || !MoveBody))
                {
                    LookAt();
                }
            }
        }
    }

    public void Interacted()
    {
        if (interactable && looked && InRange && !done)
        {
            if (OneUse)
            {
                done = true;
            }

            interacted = true;

            if (MoveHands)
            {
                if (MoveRightHand)
                {
                    MoveRHand();
                }
                if (MoveLefttHand)
                {
                    MoveLHand();
                }
            }
        }
    }

    void MoveRHand()
    {
        Handcontroller.MoveRFinger = true;
        Handcontroller.RightHandTarget = RightHandFollowPosition;

        Handcontroller.RPouce = RightHand.Pouce.transform;
        Handcontroller.RIndex = RightHand.Index.transform;
        Handcontroller.RMajeur = RightHand.Majeur.transform;
        Handcontroller.RAnnulaire = RightHand.Annulaire.transform;
        Handcontroller.RAuriculaire = RightHand.Auriculaire.transform;
    }

    void MoveLHand()
    {
        Handcontroller.MoveLFinger = true;
        Handcontroller.LeftHandTarget = LeftHandFollowPosition;

        Handcontroller.LPouce = LeftHand.Pouce.transform;
        Handcontroller.LIndex = LeftHand.Index.transform;
        Handcontroller.LMajeur = LeftHand.Majeur.transform;
        Handcontroller.LAnnulaire = LeftHand.Annulaire.transform;
        Handcontroller.LAuriculaire = LeftHand.Auriculaire.transform;
    }

    void LookAt()
    {
        cameraController.BlockRotation = true;
        cameraController.LookAtTarget = LookFollowPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ItemRange")
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

    public void LaunchVoice()
    {
        Voice.Instance.LaunchVoice(VoiceClip[VoiceNumber], VoiceLine[VoiceNumber]);
    }

    public void RunFunction()
    {
        OtherFunctions.Invoke();
    }

    public void ChangeNarrat()
    {
        Narration.Instance.Objects[NarrationChangedString] = true;
    }

    public void Release()
    {       
        interacted = false;
        bodyController.BlockMove = false;

        cameraController.LookAtTarget = null;
        cameraController.BlockRotation = false;

        Handcontroller.MoveLFinger = false;
        Handcontroller.MoveRFinger = false;

        Handcontroller.LeftHandTarget = null;
        Handcontroller.LeftHandWeight = 0;
        Handcontroller.RightHandTarget = null;
        Handcontroller.RightHandWeight = 0;

        AnimBlock = false;

        bodyController.InAnim=false;
    }

    public void Hold()
    {
        interacted = false;
        AnimBlock = false;

        bodyController.InAnim= false;
    }
}
