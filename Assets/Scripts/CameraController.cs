using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController _instance;
    public static CameraController Instance { get { return _instance; } }
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


    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;


    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;

    public Transform characterRotation;
    public Transform cam;
    public Animator Anim;

    public float cacheRot;
    public Quaternion oldRot;
    public Quaternion actualRot;

    public Transform LookAtTarget;
    public bool BlockRotation;


    private void Start()
    {
        m_CharacterTargetRot = characterRotation.rotation;
        m_CameraTargetRot = cam.localRotation;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {

        if (!BlockRotation)
        {
            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;


            actualRot = transform.rotation;
            cacheRot = Quaternion.Angle(actualRot, oldRot);
            oldRot = transform.rotation;


            Anim.SetFloat("RotSpeed", cacheRot);

            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
            {
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);
            }


            if (smooth)
            {
                characterRotation.rotation = Quaternion.Slerp(characterRotation.rotation, m_CharacterTargetRot,
                        smoothTime * Time.deltaTime);
                cam.localRotation = Quaternion.Slerp(cam.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                characterRotation.rotation = m_CharacterTargetRot;
                cam.localRotation = m_CameraTargetRot;
            }
        }
        else
        {

            var lookPos = LookAtTarget.position - characterRotation.position;
            lookPos.y = 0;

            m_CharacterTargetRot = Quaternion.LookRotation(lookPos);
            characterRotation.rotation = Quaternion.Slerp(characterRotation.rotation, m_CharacterTargetRot, 2 * Time.deltaTime);

            Quaternion _lookRotation = Quaternion.LookRotation((LookAtTarget.position - cam.position).normalized);
            m_CameraTargetRot = Quaternion.Euler(_lookRotation.eulerAngles.x, cam.localRotation.eulerAngles.y, cam.localRotation.eulerAngles.z);
            cam.localRotation = Quaternion.Slerp(cam.localRotation, m_CameraTargetRot, 2 * Time.deltaTime);

        }
   
    }


    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
