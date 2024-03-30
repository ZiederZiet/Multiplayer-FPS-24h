using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private static float MOVEMENT_SPEED = 24F;
    private static float AIR_MOVEMENT_SPEED = 8F;
    //private static float MAX_MOVEMENT_SPEED = 11F;
    private static float JUMP_POWER = 11F;

    private Rigidbody m_rb;

    private Animator m_animator;

    private float m_horizontalInput;
    private float m_verticalInput;

    private bool m_ground;

    private float m_jumpCouldown;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();

        m_animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (IsOwner)
        {
            m_horizontalInput = Input.GetAxisRaw("Horizontal");
            m_verticalInput = Input.GetAxisRaw("Vertical");

            if (m_jumpCouldown > 0F)
            {
                m_jumpCouldown -= Time.deltaTime;
            }

            if (m_ground && Input.GetKey(KeyCode.Space) && m_jumpCouldown <= 0F)
            {
                m_rb.velocity += Vector3.up * JUMP_POWER;
                m_ground = false;
                m_jumpCouldown = 0.4F;
            }
        }
    }
    private void FixedUpdate()
    {
        if (IsOwner)
        {
            float speed = MOVEMENT_SPEED;
            if (!m_ground)
            {
                speed = AIR_MOVEMENT_SPEED;
            }

            m_rb.velocity += (transform.right * m_horizontalInput + transform.forward * m_verticalInput).normalized * speed * Time.deltaTime;

            m_animator.SetBool("Walking", Mathf.Abs(m_horizontalInput) + Mathf.Abs(m_verticalInput) > 0.3F);
        }

        //float speed = GetSpeed();
        //if (speed >= MAX_MOVEMENT_SPEED)
        //{
        //    m_rb.velocity = new Vector3(m_rb.velocity.x / speed * MAX_MOVEMENT_SPEED, m_rb.velocity.y, m_rb.velocity.z / speed * MAX_MOVEMENT_SPEED);
        //}
    }
    private float GetSpeed()
    {
        return Mathf.Sqrt((m_rb.velocity.x * m_rb.velocity.x) + (m_rb.velocity.y * m_rb.velocity.y));
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_ground = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        m_ground = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        m_ground = true;
    }
}
