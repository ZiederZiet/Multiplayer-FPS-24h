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

    private float m_horizontalInput;
    private float m_verticalInput;

    private bool m_ground;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (IsOwner)
        {
            m_horizontalInput = Input.GetAxisRaw("Horizontal");
            m_verticalInput = Input.GetAxisRaw("Vertical");

            if (m_ground && Input.GetKeyDown(KeyCode.Space))
            {
                m_rb.velocity += Vector3.up * JUMP_POWER;
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
}
