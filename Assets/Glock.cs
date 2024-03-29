using FishNet.Component.Animating;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : NetworkBehaviour
{
    private static int MAX_AMMO = 18;

    private static float MAX_SHOOTING_COULDOWN = 0.3F;
    private static float MAX_RELOADING_COULDOWN = 0.9F;

    private static float DAMAGE = 25F;

    [SerializeField] private PlayerView m_playerView;

    private int m_ammo;
    private Animator m_animator;
    private NetworkAnimator m_networkAnimator;

    private float m_shootingCouldown;
    private float m_reloadingCouldowm;

    private TMPro.TMP_Text m_ammoText;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_networkAnimator = GetComponent<NetworkAnimator>();
        m_ammoText = HudManager.Singleton.GetAmmoTextMesh();
        m_ammo = 18;
        RefreshAmmoText();
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
        RefreshAmmoText();
        m_shootingCouldown = MAX_SHOOTING_COULDOWN;
        m_networkAnimator.SetTrigger("Shoot");
        m_animator.SetTrigger("Shoot");

        if (m_playerView.TryRaycastFromHead(out RaycastHit hit))
        {
            if (hit.transform.TryGetComponent<Player>(out Player hitPlayer))
            {
                hitPlayer.Damage(DAMAGE);
                HudManager.Singleton.HitMark();
            }
        }
    }

    private void Reload()
    {
        m_ammo = MAX_AMMO;
        RefreshAmmoText();
        m_reloadingCouldowm = MAX_RELOADING_COULDOWN;
        m_networkAnimator.SetTrigger("Reload");
        m_animator.SetTrigger("Reload");
    }

    private void RefreshAmmoText()
    {
        m_ammoText.text = m_ammo.ToString() + "/" + MAX_AMMO.ToString();
    }
}
