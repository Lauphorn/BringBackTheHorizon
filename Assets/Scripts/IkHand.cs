using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

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
    public FullBodyBipedIK FullBodyIK;
    public float CrouchWeight;

    public AnimationCurve myCurve;
    //float y = this.myCurve.Evaluate(x);


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
            avatar.SetLayerWeight(2, CrouchWeight);

            // RIGHT //
            if (RightHandTarget != null)
            {
                avatar.SetIKPositionWeight(AvatarIKGoal.RightHand, RightHandWeight);
                avatar.SetIKRotationWeight(AvatarIKGoal.RightHand, RightHandWeight);
                avatar.SetIKPosition(AvatarIKGoal.RightHand, RightHandTarget.position);
                avatar.SetIKRotation(AvatarIKGoal.RightHand, RightHandTarget.rotation * Quaternion.Euler(-90, 90f, 0));
                FullBodyIK.solver.rightHandEffector.target = RightHandTarget;
                FullBodyIK.solver.rightHandEffector.positionWeight = RightHandWeight;
                //FullBodyIK.solver.rightHandEffector.rotationWeight = RightHandWeight;
                //FullBodyIK.solver.rightArmChain.bendConstraint.weight = RightHandWeight;
                //FullBodyIK.solver.rightArmMapping.weight = RightHandWeight;
            }

            if (MoveRFinger)
            {
                if (RPouce != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightThumbProximal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.RightThumbProximal).localRotation, RPouce.localRotation,RightHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightThumbIntermediate, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.RightThumbIntermediate).localRotation, RPouce.GetChild(0).localRotation, RightHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightThumbDistal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.RightThumbDistal).localRotation, RPouce.GetChild(0).GetChild(0).localRotation, RightHandWeight));
                }

                if (RIndex != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightIndexProximal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.RightIndexProximal).localRotation, RIndex.localRotation, RightHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightIndexIntermediate, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.RightIndexIntermediate).localRotation, RIndex.GetChild(0).localRotation, RightHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightIndexDistal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.RightIndexDistal).localRotation, RIndex.GetChild(0).GetChild(0).localRotation, RightHandWeight));
                }

                if (RMajeur != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightMiddleProximal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.RightMiddleProximal).localRotation, RMajeur.localRotation,RightHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightMiddleIntermediate, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate).localRotation, RMajeur.GetChild(0).localRotation,RightHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightMiddleDistal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.RightMiddleDistal).localRotation, RMajeur.GetChild(0).GetChild(0).localRotation,RightHandWeight));
                }

                if (RAnnulaire != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightRingProximal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.RightRingProximal).localRotation, RAnnulaire.localRotation,RightHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightRingIntermediate, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.RightRingIntermediate).localRotation, RAnnulaire.GetChild(0).localRotation,RightHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightRingDistal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.RightRingDistal).localRotation, RAnnulaire.GetChild(0).GetChild(0).localRotation,RightHandWeight));
                }

                if (RAuriculaire != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightLittleProximal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.RightLittleProximal).localRotation, RAuriculaire.localRotation,RightHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightLittleIntermediate, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.RightLittleIntermediate).localRotation, RAuriculaire.GetChild(0).localRotation,RightHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.RightLittleDistal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.RightLittleDistal).localRotation, RAuriculaire.GetChild(0).GetChild(0).localRotation,RightHandWeight));
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
                FullBodyIK.solver.leftHandEffector.target = LeftHandTarget;
                FullBodyIK.solver.leftHandEffector.positionWeight = LeftHandWeight;
                //FullBodyIK.solver.leftHandEffector.rotationWeight = LeftHandWeight;
                //FullBodyIK.solver.leftArmChain.bendConstraint.weight = LeftHandWeight;
                //FullBodyIK.solver.leftArmMapping.weight = LeftHandWeight;
            }

            if (MoveLFinger)
            {
                if (LPouce != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftThumbProximal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.LeftThumbProximal).localRotation, LPouce.localRotation, LeftHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftThumbIntermediate, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.LeftThumbIntermediate).localRotation, LPouce.GetChild(0).localRotation, LeftHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftThumbDistal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.LeftThumbDistal).localRotation, LPouce.GetChild(0).GetChild(0).localRotation, LeftHandWeight));
                }

                if (LIndex != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftIndexProximal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.LeftIndexProximal).localRotation, LIndex.localRotation, LeftHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftLittleIntermediate, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.LeftLittleIntermediate).localRotation, LIndex.GetChild(0).localRotation, LeftHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftIndexDistal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.LeftIndexDistal).localRotation, LIndex.GetChild(0).GetChild(0).localRotation, LeftHandWeight));
                }

                if (LMajeur != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftMiddleProximal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.LeftMiddleProximal).localRotation, LMajeur.localRotation, LeftHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftMiddleIntermediate, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.LeftMiddleIntermediate).localRotation, LMajeur.GetChild(0).localRotation, LeftHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftMiddleDistal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.LeftMiddleDistal).localRotation, LMajeur.GetChild(0).GetChild(0).localRotation, LeftHandWeight));
                }

                if (LAnnulaire != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftRingProximal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.LeftRingProximal).localRotation, LAnnulaire.localRotation, LeftHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftRingIntermediate, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.LeftRingIntermediate).localRotation, LAnnulaire.GetChild(0).localRotation, LeftHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftRingDistal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.LeftRingDistal).localRotation, LAnnulaire.GetChild(0).GetChild(0).localRotation, LeftHandWeight));
                }

                if (LAuriculaire != null)
                {
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftLittleProximal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.LeftLittleProximal).localRotation, LAuriculaire.localRotation, LeftHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftLittleIntermediate, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.LeftLittleIntermediate).localRotation, LAuriculaire.GetChild(0).localRotation, LeftHandWeight));
                    avatar.SetBoneLocalRotation(HumanBodyBones.LeftLittleDistal, Quaternion.Slerp(avatar.GetBoneTransform(HumanBodyBones.LeftLittleDistal).localRotation, LAuriculaire.GetChild(0).GetChild(0).localRotation, LeftHandWeight));
                }
            }
            // END LEFT//

        }
    }
}
