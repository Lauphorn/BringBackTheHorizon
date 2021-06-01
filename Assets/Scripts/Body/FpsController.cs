using System.Collections;
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
    [HideInInspector]
    public Transform BodyFollowPosition;

    private float z, x, YRot;
    private bool RRot, LRot;
 
    Vector3 _direction;
    public float rotationDamping, moveDamping;

    public Animator Anim;
    public bool InAnim, pause;

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
            if (!pause)
            {
                Move();
                MoveDone = false;
            }
        }
        else
        {
            FollowTargetWitouthRotation(BodyFollowPosition, 0.2f, 300f);
        }


        CheckStair();

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
            }
            else
            {
                LRot = false;
                Anim.SetBool("TurnLeft", false);
            }
        }

        Anim.SetFloat("x", Mathf.Lerp(Anim.GetFloat("x"), Rotation.InverseTransformDirection(m_Rigidbody.velocity).x, moveDamping));
        Anim.SetFloat("z", Mathf.Lerp(Anim.GetFloat("z"), Rotation.InverseTransformDirection(m_Rigidbody.velocity).z, moveDamping));
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
            m_Rigidbody.AddRelativeForce(direction.normalized * (speed *Time.deltaTime), ForceMode.Force);

            MoveDone = false;
        }
        else
        {
            m_Rigidbody.position = Vector3.MoveTowards(m_Rigidbody.position, target.position, 0.005f);
            MoveDone = true;

        }
    }

    private void FixedUpdate()
    {
        if (!BlockMove)
        {
            m_Rigidbody.AddForce(_direction * m_Acceleration, ForceMode.Acceleration);
        }
        else
        {

        }

        if (m_Rigidbody.useGravity)
        {
            m_Rigidbody.AddForce(Physics.gravity * 10, ForceMode.Acceleration);
        }
    }

    private void LateUpdate()
    {
        if (BlockMove)
        {
            Rotation.rotation = Quaternion.RotateTowards(Rotation.rotation, Cam.rotation, 2);
        }

        if (RRot || LRot)
        {
            Rotation.rotation = Quaternion.Slerp(Rotation.rotation, Cam.rotation, 0.1f);
        }
    }

    public void CheckStair()
    {
        int layerMask = 1 << 12;
        // This would cast rays only against colliders in layer 12

        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down),Color.blue, 1);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1, layerMask))
        {
            m_MaxWalkSpeed = 1f;
            Anim.SetBool("Stairs", true);
        }
        else
        {
            m_MaxWalkSpeed = 1.9f;
            Anim.SetBool("Stairs", false);
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
}

