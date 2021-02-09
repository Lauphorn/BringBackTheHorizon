using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
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


    public bool MoveHands;
    [ConditionalField("MoveHands")] public bool MoveRightHand;
    [ConditionalField("MoveRightHand")] public Transform RightHandFollowPosition;
    [ConditionalField("MoveRightHand")] public HandLineRenderer RightHand;
    [ConditionalField("MoveRightHand")] public float RightHandWeight;


    [ConditionalField("MoveHands")] public bool MoveLefttHand;
    [ConditionalField("MoveLefttHand")] public Transform LeftHandFollowPosition;
    [ConditionalField("MoveLefttHand")] public HandLineRenderer LeftHand;
    [ConditionalField("MoveLefttHand")] public float LeftHandWeight;


    public Animator Anim;
    bool Interactable, interacted, done;
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
        if (interacted)
        {
            if (MoveBody)
            {
                bodyController.BlockMove = true;
                bodyController.FollowTargetWitouthRotation(BodyFollowPosition, 0.05f, 7);
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
                if (bodyController.MoveDone == true)
                {
                    Anim.SetTrigger("Interact");
                    if (LaunchAnim)
                    {
                        IkHand.Instance.LaunchAnim(AnimBool);
                    }
                    AnimBlock = true;
                }

                if (LookAtTarget && bodyController.MoveDone == true)
                {
                    LookAt();
                }
            }
        }
    }

    public void Interacted()
    {
        if (Interactable && !done)
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
            Interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ItemRange")
        {
            Interactable = false;
        }
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
    }

    public void Hold()
    {
        interacted = false;
        AnimBlock = false;
    }


}
