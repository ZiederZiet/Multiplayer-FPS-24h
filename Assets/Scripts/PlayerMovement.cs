using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private static float MOVEMENT_SPEED = 13F;
    //private static float MAX_MOVEMENT_SPEED = 11F;
    private static float JUMP_POWER = 8F;

    private Rigidbody m_rb;

    private float m_horizontalInput;
    private float m_verticalInput;
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_rb.velocity += Vector3.up * JUMP_POWER;
            }
        }
    }
    private void FixedUpdate()
    {
        m_rb.velocity += (transform.right * m_horizontalInput + transform.forward * m_verticalInput).normalized * MOVEMENT_SPEED * Time.deltaTime;

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
}
