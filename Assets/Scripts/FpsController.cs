﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsController : MonoBehaviour
{

    private static FpsController _instance;
    public static FpsController Instance { get { return _instance; } }
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

    [Tooltip("How fast the player moves when walking (default move speed).")]
    [SerializeField]
    private float m_Acceleration = 6.0f;

    [Tooltip("How fast the player moves when walking (default move speed).")]
    [SerializeField]
    private float m_MaxWalkSpeed = 6.0f;

    [Tooltip("How fast the player moves when running.")]
    [SerializeField]
    private float m_MaxRunSpeed = 11.0f;

    private Rigidbody m_Rigidbody;

    public Transform Cam;
    public Transform Rotation;

    [HideInInspector]
    public bool BlockMove, MoveDone;

    private float z, x, YRot;
    private bool RRot, LRot;
 
    Vector3 _direction;
    public float rotationDamping;

    public Animator Anim;

    private void Start()
    {
        // Saving component references to improve performance.
        m_Rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        if (!BlockMove)
        {
            Move();
        }

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if (m_Rigidbody.velocity.magnitude > 0.1f)
        {
            Rotation.rotation = Quaternion.Slerp(Rotation.rotation, Cam.rotation, rotationDamping);
            Anim.SetBool("TurnRight", false);
            Anim.SetBool("TurnLeft", false);
        }
        else
        {
            float angleA = Rotation.rotation.eulerAngles.y;
            float angleB = Cam.rotation.eulerAngles.y;
            YRot = Mathf.DeltaAngle(angleA, angleB);

            if (YRot > 50 || (RRot && YRot>10))
            {
                RRot = true;
                Anim.SetBool("TurnRight", true);
                Rotation.rotation = Quaternion.Slerp(Rotation.rotation, Cam.rotation, rotationDamping/2);
            }
            else
            {
                RRot = false;
                Anim.SetBool("TurnRight", false);
            }

            if (YRot < -50 || (LRot && YRot < -10))
            {
                LRot = true;
                Anim.SetBool("TurnLeft", true);
                Rotation.rotation = Quaternion.Slerp(Rotation.rotation, Cam.rotation, rotationDamping/2);
            }
            else
            {
                LRot = false;
                Anim.SetBool("TurnLeft", false);
            }
        }


        IkHead.Instance.FootWeight = 1 - m_Rigidbody.velocity.magnitude;



        Anim.SetFloat("x", Rotation.InverseTransformDirection(m_Rigidbody.velocity).x);
        Anim.SetFloat("z", Rotation.InverseTransformDirection(m_Rigidbody.velocity).z);
        Anim.SetFloat("Speed", m_Rigidbody.velocity.magnitude);
    }

    public void Move()
    {
        _direction = Rotation.right * x + Rotation.forward * z;

        if (m_Rigidbody.velocity.magnitude > m_MaxWalkSpeed)
        {
            m_Rigidbody.velocity = m_Rigidbody.velocity.normalized * m_MaxWalkSpeed;
        }
    }

    public void FollowTargetWitouthRotation(Transform target, float distanceToStop, float speed)
    {
        var direction = Vector3.zero;
        if (Vector3.Distance(transform.position, target.position) > distanceToStop)
        {
            
            direction = target.position - transform.position;
            m_Rigidbody.AddRelativeForce(direction.normalized * speed, ForceMode.Force);

            MoveDone = false;
        }
        else
        {
            MoveDone = true;
            m_Rigidbody.position = Vector3.Lerp(m_Rigidbody.position, target.position, 10 * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (!BlockMove)
        {
            m_Rigidbody.AddForce(_direction * m_Acceleration, ForceMode.Acceleration);
        }

        if (m_Rigidbody.useGravity)
        {
            m_Rigidbody.AddForce(Physics.gravity * 10, ForceMode.Acceleration);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Stairs")
        {
            m_Rigidbody.useGravity = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Stairs")
        {
            m_Rigidbody.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Stairs")
        {
            m_MaxWalkSpeed = 1.3f;
            Anim.SetBool("Stairs", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Stairs")
        {
            m_MaxWalkSpeed = 1.9f;
            Anim.SetBool("Stairs", false);
        }
    }
}

