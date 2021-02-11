using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkHand : MonoBehaviour
{

    private static IkHand _instance;
    public static IkHand Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    public Animator avatar;

    public Transform RightHandTarget;

    public float RightHandWeight;

    [HideInInspector]
    public Transform RPouce;
    [HideInInspector]
    public Transform RIndex;
    [HideInInspector]
    public Transform RMajeur;
    [HideInInspector]
    public Transform RAnnulaire;
    [HideInInspector]
    public Transform RAuriculaire;

    public Transform LeftHandTarget;
    public float LeftHandWeight;

    [HideInInspector]
    public Transform LPouce;
    [HideInInspector]
    public Transform LIndex;
    [HideInInspector]
    public Transform LMajeur;
    [HideInInspector]
    public Transform LAnnulaire;
    [HideInInspector]
    public Transform LAuriculaire;

    [HideInInspector]
    public bool MoveRFinger, MoveLFinger;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchAnim(string AnimName)
    {
        avatar.SetTrigger(AnimName);
    }


    private void OnAnimatorIK(int layerIndex)
    {
        if (avatar)
        {

            // RIGHT //
            if (RightHandTarget != null)
            {
                avatar.SetIKPositionWeight(AvatarIKGoal.RightHand, RightHandWeight);
                avatar.SetIKRotationWeight(AvatarIKGoal.RightHand, RightHandWeight);
                avatar.SetIKPosition(AvatarIKGoal.RightHand, RightHandTarget.position);
                avatar.SetIKRotation(AvatarIKGoal.RightHand, RightHandTarget.rotation * Quaternion.Euler(-90, 90f, 0));
            }

            if (MoveRFinger)
            {
                if (RPouce != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightThumbProximal, RPouce.localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightThumbIntermediate, RPouce.GetChild(0).localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightThumbDistal, RPouce.GetChild(0).GetChild(0).localRotation);
                }

                if (RIndex != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightIndexProximal, RIndex.localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightIndexIntermediate, RIndex.GetChild(0).localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightIndexDistal, RIndex.GetChild(0).GetChild(0).localRotation);
                }

                if (RMajeur != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightMiddleProximal, RMajeur.localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightMiddleIntermediate, RMajeur.GetChild(0).localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightMiddleDistal, RMajeur.GetChild(0).GetChild(0).localRotation);
                }

                if (RAnnulaire != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightRingProximal, RAnnulaire.localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightRingIntermediate, RAnnulaire.GetChild(0).localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightRingDistal, RAnnulaire.GetChild(0).GetChild(0).localRotation);
                }

                if (RAuriculaire != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightLittleProximal, RAuriculaire.localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightLittleIntermediate, RAuriculaire.GetChild(0).localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightLittleDistal, RAuriculaire.GetChild(0).GetChild(0).localRotation);
                }
            }

            // END RIGHT //


            // LEFT //
            if (LeftHandTarget != null)
            {
                avatar.SetIKPositionWeight(AvatarIKGoal.LeftHand, LeftHandWeight);
                avatar.SetIKRotationWeight(AvatarIKGoal.LeftHand, LeftHandWeight);
                avatar.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandTarget.position);
                avatar.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandTarget.rotation * Quaternion.Euler(-90, -90f, 0));
            }

            if (MoveLFinger)
            {
                if (LPouce != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftThumbProximal, LPouce.localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftThumbIntermediate, LPouce.GetChild(0).localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftThumbDistal, LPouce.GetChild(0).GetChild(0).localRotation);
                }

                if (LIndex != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftIndexProximal, LIndex.localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftIndexIntermediate, LIndex.GetChild(0).localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftIndexDistal, LIndex.GetChild(0).GetChild(0).localRotation);
                }

                if (LMajeur != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftMiddleProximal, LMajeur.localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftMiddleIntermediate, LMajeur.GetChild(0).localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftMiddleDistal, LMajeur.GetChild(0).GetChild(0).localRotation);
                }

                if (LAnnulaire != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftRingProximal, LAnnulaire.localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftRingIntermediate, LAnnulaire.GetChild(0).localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftRingDistal, LAnnulaire.GetChild(0).GetChild(0).localRotation);
                }

                if (LAuriculaire != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftLittleProximal, LAuriculaire.localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftLittleIntermediate, LAuriculaire.GetChild(0).localRotation);
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftLittleDistal, LAuriculaire.GetChild(0).GetChild(0).localRotation);
                }
            }
            // END LEFT//

        }
    }
}
