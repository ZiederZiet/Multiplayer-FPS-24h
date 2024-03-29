using FishNet.Component.Animating;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : NetworkBehaviour
{
    private static int MAX_AMMO = 12;

    private static float MAX_SHOOTING_COULDOWN = 0.3F;
    private static float MAX_RELOADING_COULDOWN = 1.45F;

    private static float DAMAGE = 30F;

    [SerializeField] private PlayerView m_playerView;

    [SerializeField] private AudioSource m_audioSource;

    [SerializeField] private LineRenderer m_tracer;

    [SerializeField] private Light m_tracerLight;

    private float m_tracerTimer;

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
        m_ammo = MAX_AMMO;
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
                if (m_reloadingCouldowm <= 0f)
                {
                    RefreshAmmoText();
                }
            }
            if (m_shootingCouldown <= 0F && m_reloadingCouldowm <= 0F)
            {
                if (Input.GetMouseButtonDown(0))
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
                else if (m_ammo < MAX_AMMO && Input.GetKeyDown(KeyCode.R))
                {
                    Reload();
                }
            }
        }
        if (m_tracerTimer > 0F)
        {
            m_tracerTimer -= Time.deltaTime;
            if (m_tracerTimer <= 0F)
            {
                m_tracer.enabled = false;
                m_tracerLight.enabled = false;
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

        float maxDistance = 100F;
        if (m_playerView.TryRaycastFromHead(out RaycastHit hit, maxDistance))
        {
            if (hit.transform.TryGetComponent<Player>(out Player hitPlayer))
            {
                hitPlayer.Damage(DAMAGE);
                HudManager.Singleton.HitMark();
            }
        }

        ServerShoot(m_playerView.RaycastPositionFromHead(maxDistance));
    }

    [ServerRpc]
    private void ServerShoot(Vector3 hitPosition)
    {
        ObserverShoot(hitPosition);
    }

    [ObserversRpc]
    private void ObserverShoot(Vector3 hitPosition)
    {
        m_audioSource.Stop();
        m_audioSource.Play();
        m_tracer.enabled = true;
        m_tracerLight.enabled = true;
        m_tracerTimer = 0.04F;
        m_tracer.SetPosition(0, m_audioSource.transform.position);
        m_tracer.SetPosition(1, hitPosition);
    }

    private void Reload()
    {
        m_ammo = MAX_AMMO;
        m_reloadingCouldowm = MAX_RELOADING_COULDOWN;
        m_networkAnimator.SetTrigger("Reload");
        m_animator.SetTrigger("Reload");
    }

    private void RefreshAmmoText()
    {
        m_ammoText.text = m_ammo.ToString() + "/" + MAX_AMMO.ToString();
    }

    public void ResetAmmo()
    {
        m_ammo = MAX_AMMO;
        RefreshAmmoText();
    }
}
