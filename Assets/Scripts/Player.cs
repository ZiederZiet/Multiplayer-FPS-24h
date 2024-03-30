using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private Flashlight m_flashlight;
    [SerializeField] private Glock m_glock;

    private static float MAX_HEALTH = 100F;

    private float m_health;

    private void Update()
    {
        if (IsOwner)
        {
            if (Input.GetMouseButtonDown(1))
            {
                m_flashlight.TriggerSwitch();
            }
            if (transform.position.y < -20F)
            {
                Die();
            }
        }
    }

    public void Damage(float amount)
    {
        ServerDamage(amount);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ServerDamage(float amount)
    {
        LocalDamage(amount);
    }

    [ObserversRpc]
    private void LocalDamage(float amount)
    {
        if (IsOwner)
        {
            m_health -= amount;
            if (m_health <= 0)
            {
                Die();
            }
            else
            {
                HudManager.Singleton.UpdateHealthBar(m_health, MAX_HEALTH);
            }
        }
    }

    public override void OnStartClient()
    {
        m_health = MAX_HEALTH;
        HudManager.Singleton.UpdateHealthBar(m_health, MAX_HEALTH);
    }

    public void Die()
    {
        m_health = MAX_HEALTH;
        HudManager.Singleton.UpdateHealthBar(m_health, MAX_HEALTH);
        m_glock.ResetAmmo();
        m_flashlight.ResetSwitch();
        transform.position = SpawnManager.Singleton.GetRandomSpawn().position;
    }
}
