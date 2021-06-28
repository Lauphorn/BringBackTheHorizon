using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.Events;
public class InteractableObject : MonoBehaviour
{

    FpsController bodyController;
    CameraController cameraController;
    IkHand Handcontroller;

    public bool Activated;

    public bool NeedActivation;
    [ConditionalField("NeedActivation")] public InteractableObject ParentObject;

    public bool ShowIfNarration;
    [ConditionalField("ShowIfNarration")] public Narration.Narrations ShowWithNarration;
    bool narrCheck;

    public bool HideWithGrabbedObjects;
    int Hidden;

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
    [ConditionalField("ChangeNarration")] public Narration.Narrations NarrationChanged;

    public bool NeedNarration;
    [ConditionalField("NeedNarration")] public Narration.Narrations NarrationNeeded;
    bool NarrationNeededCheck;

    public bool UseVoice;
    public List<SousTitre> Subtitle;
    [ConditionalField("UseVoice")] public int VoiceNumber;

    public bool GrabObject;
    [ConditionalField("GrabObject")] public Transform ObjectGrabbed;
    [ConditionalField("GrabObject")] public Transform GrabbingHand;

    public bool MoveHands;
    [ConditionalField("MoveHands")] public BodyHandHolder HandsHolder;
    
    HandLineRenderer RightHandRenderer;
    GameObject RightHand;
    [HideInInspector]
    public float RightHandWeight;
    HandLineRenderer LeftHandRenderer;
    GameObject LeftHand;
    [HideInInspector]
    public float LeftHandWeight;

    public enum LogoChoice
    {
        Hand,
        Eye
    }
    public LogoChoice logo;

    public Animator Anim;
    public Animator KnobAnimator;

    [HideInInspector]
    public bool InRange, interacted, done, interactable, looked, ParentActivated, InRoom, blocked, NarrationActivationCheck;

    bool AnimBlock, activatedOnce;


    // Start is called before the first frame update
    void Start()
    {
        KnobAnimator = transform.Find("UiInteraction").transform.GetComponent<Animator>();

        gameObject.AddComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;


        bodyController = FpsController.Instance;
        cameraController = CameraController.Instance;
        Handcontroller = IkHand.Instance;

        if (NeedActivation)
        {
            ParentActivated = false;
        }
        else
        {
            ParentActivated = true;
        }

        if (MoveHands)
        {
            RightHandRenderer = HandsHolder.RightHand.GetComponent<HandLineRenderer>();
            RightHand = HandsHolder.RightHand;
            LeftHandRenderer = HandsHolder.LeftHand.GetComponent<HandLineRenderer>();
            LeftHand = HandsHolder.LeftHand;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(KnobAnimator != null && ParentActivated)
        {
            if (InRange && InRoom && !bodyController.InAnim && !done && NarrationActivationCheck && Hidden==0)
            {
                KnobAnimator.SetBool("Ball", true);

                if (NeedNarration && activatedOnce)
                {
                    blocked = !Narration.Instance.CheckNarration(NarrationNeeded);
                }
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
        }

        if (NeedActivation)
        {
            ParentActivated = ParentObject.Activated;
        }


        if (ShowIfNarration)
        {
            if (Narration.Instance.NarrationHasChanged != narrCheck)
            {
                narrCheck = Narration.Instance.NarrationHasChanged;

                if (Narration.Instance.CheckNarration(ShowWithNarration))
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

        if (interacted)
        {

            bodyController.InAnim=true;

            if (Crouch)
            {
                Handcontroller.CrouchWeight = CrouchWeight;
            }

            if (MoveBody)
            {
                bodyController.FollowPosition(BodyFollowPosition);
            }

            
            if (!AnimBlock)
            {

                if (bodyController.MoveDone == true || !MoveBody)
                {

                    if (NeedNarration)
                    {
                        NarrationNeededCheck = Narration.Instance.CheckNarration(NarrationNeeded);
                        if (NarrationNeededCheck)
                        {
                            Anim.SetTrigger("Interact");
                            AnimBlock = true;
                        }
                        else
                        {
                            Anim.SetTrigger("Narration");
                            AnimBlock = true;
                        }
                    }
                    else
                    {
                        Anim.SetTrigger("Interact");
                        AnimBlock = true;
                    }

                    if (LaunchAnim)
                    {
                        Handcontroller.LaunchAnim(AnimBool);
                    }

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
        if (interactable && looked && InRange && !done && ParentActivated && NarrationActivationCheck && Hidden ==0)
        {
            interacted = true;
            activatedOnce = true;

            if (MoveHands)
            {
                MoveRHand();
                MoveLHand();
            }
        }

    }

    void MoveRHand()
    {
        Handcontroller.MoveRFinger = true;
        Handcontroller.RightHandTarget = RightHand.transform;

        Handcontroller.RPouce = RightHandRenderer.Pouce.transform;
        Handcontroller.RIndex = RightHandRenderer.Index.transform;
        Handcontroller.RMajeur = RightHandRenderer.Majeur.transform;
        Handcontroller.RAnnulaire = RightHandRenderer.Annulaire.transform;
        Handcontroller.RAuriculaire = RightHandRenderer.Auriculaire.transform;
    }

    void MoveLHand()
    {
        Handcontroller.MoveLFinger = true;
        Handcontroller.LeftHandTarget = LeftHand.transform;

        Handcontroller.LPouce = LeftHandRenderer.Pouce.transform;
        Handcontroller.LIndex = LeftHandRenderer.Index.transform;
        Handcontroller.LMajeur = LeftHandRenderer.Majeur.transform;
        Handcontroller.LAnnulaire = LeftHandRenderer.Annulaire.transform;
        Handcontroller.LAuriculaire = LeftHandRenderer.Auriculaire.transform;
    }

    void LookAt()
    {
        cameraController.FollowRotation(LookFollowPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ItemRange")
        {
            InRange = true;
        }

        if (other.tag == "GrabItem" && HideWithGrabbedObjects)
        {
            Hidden +=1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ItemRange")
        {
            InRange = false;
        }

        if (other.tag == "GrabItem" && HideWithGrabbedObjects)
        {
            Hidden -=1;
        }
    }

    void OnBecameInvisible()
    {
        Debug.Log("Invisible "+ gameObject.name);
    }

    void OnBecameVisible()
    {
        Debug.Log("Visible " + gameObject.name);
    }

    public void LaunchVoice()
    {
        Subtitle[VoiceNumber].Talk();
    }

    public void RunFunction()
    {
        OtherFunctions.Invoke();
    }

    public void ChangeNarrat()
    {
        Debug.Log("1");
        Narration.Instance.ChangeNarration(NarrationChanged,true);
    }

    public void Release()
    {
        interacted = false;
        bodyController.StopFollow();

        cameraController.StopFollowRotation();

        Handcontroller.MoveLFinger = false;
        Handcontroller.MoveRFinger = false;

        Handcontroller.LeftHandTarget = null;
        Handcontroller.RightHandTarget = null;

        AnimBlock = false;

        bodyController.InAnim=false;
    }

    public void Hold()
    {
        interacted = false;
        AnimBlock = false;

        bodyController.InAnim= false;
    }

    public void Grab()
    {
        if(ObjectGrabbed.parent != GrabbingHand)
        {
            ObjectGrabbed.SetParent(GrabbingHand, true);
        }
        else
        {
            ObjectGrabbed.parent = null;
        }
    }

    public void OneUse()
    {
        done = true;
    }

    public void SwitchActivation()
    {
        Activated = !Activated;
    }

    public void ChangeWeightLeft()
    {
        Handcontroller.WeightLeft();
    }

    public void ChangeWeightRight()
    {
        Handcontroller.WeightRight();
    }

    public void ChangeLock()
    {
        blocked = !blocked;
    }

    /*
    [ExecuteInEditMode]
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {

            //Tag and layer
            gameObject.tag = "Item";
            gameObject.layer = 11;

            //Lookat
            if (LookAtTarget && transform.Find("Lookat") == null && LookFollowPosition == null)
            {
                LookFollowPosition = Instantiate(LookatPrefab, transform).transform;
                LookFollowPosition.name = "Lookat";
                Debug.Log("Problem" + gameObject.name);
            }
            if (!LookAtTarget && LookFollowPosition != null)
            {
                StartCoroutine(Destroy(LookFollowPosition.gameObject));
            }

            //movehands
            if (MoveHands && transform.Find("Character") ==null && HandsHolder == null)
            {
                HandsHolder = Instantiate(BodyPrefab, transform).GetComponent<BodyHandHolder>();
                HandsHolder.name = "Character";
            }
            if (!MoveHands && HandsHolder != null)
            {
                StartCoroutine(Destroy(HandsHolder.gameObject));
            }

            //uiKnob
            if (KnobAnimator == null && transform.Find("UiInteraction") == null )
            {
                KnobAnimator = Instantiate(UiInteractionPrefab, transform).GetComponent<Animator>();
                KnobAnimator.name = "UiInteraction";
            }
            if (Anim == null && GetComponent<Animator>() ==null)
            {
                Anim = gameObject.AddComponent<Animator>();
            }

            if (Zone == null && transform.Find("Zone") == null)
            {
                Zone = Instantiate(ZonePrefab, transform).GetComponent<ObjectZone>();
                Zone.obj = this;
                Zone.name = "Zone";
            }

            //uiKnob
            if (MoveBody && transform.Find("BodyPos") == null && BodyFollowPosition == null)
            {
                BodyFollowPosition = Instantiate(BodyPosPrefab, transform).transform;
                BodyFollowPosition.name = "BodyPos";
            }
            if (!MoveBody && BodyFollowPosition != null)
            {
                StartCoroutine(Destroy(BodyFollowPosition.gameObject));
            }

        }

    }

    IEnumerator Destroy(GameObject go)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }*/
}
