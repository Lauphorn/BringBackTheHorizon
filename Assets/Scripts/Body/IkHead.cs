using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkHead : MonoBehaviour
{
    private static IkHead _instance;

    public static IkHead Instance { get { return _instance; } }


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
    public Transform LookAt;

    public float lookAtWeight;
    public float LeftFootWeight, RightFootWeight;
    public float FootDamping;

    [Range(0, 1f)]
    public float DistanceToGround;



    void OnAnimatorIK(int layerIndex)
    {
        if (avatar)
        {
            avatar.SetLookAtWeight(lookAtWeight);

            if (LookAt != null)
            {
                avatar.SetLookAtPosition(LookAt.position);
            }

            // FOOT //

            // Set the weights of left and right feet to the current value defined by the curve in our animations.
            avatar.SetIKPositionWeight(AvatarIKGoal.LeftFoot, LeftFootWeight);
            avatar.SetIKRotationWeight(AvatarIKGoal.LeftFoot, LeftFootWeight);
            avatar.SetIKPositionWeight(AvatarIKGoal.RightFoot, RightFootWeight);
            avatar.SetIKRotationWeight(AvatarIKGoal.RightFoot, RightFootWeight);


            // Left Foot
            RaycastHit hit;
            // We cast our ray from above the foot in case the current terrain/floor is above the foot position.
            Ray ray = new Ray(avatar.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up*0.5f, Vector3.down);


            Debug.DrawRay(avatar.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down, Color.red);


            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f))
            {

                // We're only concerned with objects that are tagged as "Walkable"
                if (hit.transform.tag == "Walkable")
                {
                    Vector3 LTarget1 = hit.point; // The target foot position is where the raycast hit a walkable object...
                    LTarget1.y += DistanceToGround; // ... taking account the distance to the ground we added above.   

                    avatar.SetIKPosition(AvatarIKGoal.LeftFoot, LTarget1);
                    avatar.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }
            }


            // Right Foot
            ray = new Ray(avatar.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f))
            {

                if (hit.transform.tag == "Walkable")
                {
                    Vector3 RTarget1 = hit.point; // The target foot position is where the raycast hit a walkable object...
                    RTarget1.y += DistanceToGround; // ... taking account the distance to the ground we added above.

                    avatar.SetIKPosition(AvatarIKGoal.RightFoot, RTarget1);
                    avatar.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }
            }

            // END FOOT
        }
    }
}
