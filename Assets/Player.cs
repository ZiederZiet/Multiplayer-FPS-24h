using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private static Vector3 FIRST_PERSON_FLASHLIGHT_POSITION = new Vector3(-0.422F, -0.271F, 0.539F);
    private static Vector3 FIRST_PERSON_GLOCK_POSITION = new Vector3(0.426F, -0.341F, 0.461F);

    private static float SENSITIVITY = 1F;

    //[SerializeField] private GameObject[] m_nonLocalOnly;
    [SerializeField] private Transform m_head;
    [SerializeField] private Renderer m_renderer;

    [SerializeField] private Flashlight m_flashlight;
    [SerializeField] private Glock m_glock;

    private Transform m_camera;
    private float headX;
    void Start()
    {
        m_camera = Camera.main.transform;
    }
    void Update()
    {
        if (IsOwner)
        {
            m_camera.position = m_head.position;
            m_camera.rotation = m_head.rotation;

            float mouseX = Input.GetAxisRaw("Mouse X");
            float mouseY = Input.GetAxisRaw("Mouse Y");

            transform.localEulerAngles = new Vector3(0F, transform.localEulerAngles.y + (mouseX * SENSITIVITY), 0F);

            headX = Mathf.Clamp(headX - (mouseY * SENSITIVITY), -90F, 90F);
            m_head.localEulerAngles = new Vector3(headX, 0F, 0F);

            if (Input.GetMouseButtonDown(1))
            {
                m_flashlight.TriggerSwitch();
            }
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        //for (int i = 0; i < m_nonLocalOnly.Length; i++)
        //{
        //    m_nonLocalOnly[i].SetActive(IsOwner);
        //}
        if (IsOwner)
        {
            m_renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;

            m_glock.transform.localPosition = FIRST_PERSON_GLOCK_POSITION;
            m_flashlight.transform.localPosition = FIRST_PERSON_FLASHLIGHT_POSITION;
        }
    }
}
