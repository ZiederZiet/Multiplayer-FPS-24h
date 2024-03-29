using FishNet.Component.Animating;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : NetworkBehaviour
{
    private static int MAX_AMMO = 10;

    private static float MAX_SHOOTING_COULDOWN = 0.3F;
    private static float MAX_RELOADING_COULDOWN = 0.9F;

    private int m_ammo;
    private Animator m_animator;
    private NetworkAnimator m_networkAnimator;

    private float m_shootingCouldown;
    private float m_reloadingCouldowm;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_networkAnimator = GetComponent<NetworkAnimator>();
    }

    void Update()
    {
        if (IsOwner)
        {
            if (m_shootingCouldown > 0F)
            {
                m_shootingCouldown -= Time.deltaTime;
            }

            if (m_reloadingCouldowm > 0F)
            {
                m_reloadingCouldowm -= Time.deltaTime;
            }

            if (Input.GetMouseButtonDown(0) && m_shootingCouldown <= 0F && m_reloadingCouldowm <= 0F)
            {
                if (m_ammo > 0)
                {
                    Shoot();
                }
                else
                {
                    Reload();
                }
            }
        }
    }

    private void Shoot()
    {
        m_ammo--;
        m_shootingCouldown = MAX_SHOOTING_COULDOWN;
        m_networkAnimator.SetTrigger("Shoot");
        m_animator.SetTrigger("Shoot");
    }

    private void Reload()
    {
        m_ammo = MAX_AMMO;
        m_reloadingCouldowm = MAX_RELOADING_COULDOWN;
        m_networkAnimator.SetTrigger("Reload");
        m_animator.SetTrigger("Reload");
    }
}
