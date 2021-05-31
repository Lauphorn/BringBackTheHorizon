using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;


[ExecuteInEditMode]
public class IkAnimBody : MonoBehaviour
{
    public Animator avatar;

    public bool UseRightHand;
    [ConditionalField("UseRightHand")] public Transform RightHandPos;

    public bool UseLeftHand;
    [ConditionalField("UseLeftHand")] public Transform LeftHandPos;

    private void OnAnimatorIK(int layerIndex)
    {
        if (avatar)
        {
            // RIGHT //
            if (UseRightHand && RightHandPos != null)
            {
                avatar.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                avatar.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                avatar.SetIKPosition(AvatarIKGoal.RightHand, RightHandPos.position);
                avatar.SetIKRotation(AvatarIKGoal.RightHand, RightHandPos.rotation * Quaternion.Euler(90, 90f, 0));
            }

            // Left //
            if (UseLeftHand && LeftHandPos != null)
            {
                avatar.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                avatar.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                avatar.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandPos.position);
                avatar.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandPos.rotation * Quaternion.Euler(-90, -90f, 0));
            }
        }
    }

    private void Update()
    {
        avatar.Update(0);
    }
}
