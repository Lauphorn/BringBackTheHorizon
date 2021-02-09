using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Follow : MonoBehaviour
{
    public Transform TargetRotation;

    public float Damp;

    public Transform TargetPosition;

    public Vector3 Offset;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition.position+Offset, Damp);
        //Rotation.eulerAngles = new Vector3(0, TargetRotation.transform.eulerAngles.y, 0);
    }

}
