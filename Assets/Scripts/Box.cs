using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : NetworkBehaviour
{
    private static float DISOLVING_TIME = 120F;

    private Transform m_parent;

    private float m_disolvingTimer;

    private int health;

    public void SetParent(Transform parent)
    {
        m_parent = parent;
    }

    void Start()
    {
        m_disolvingTimer = DISOLVING_TIME / 3F;
        health = 3;
    }
    void Update()
    {
        if (IsOwner)
        {
            m_disolvingTimer -= 0F;
            if (m_disolvingTimer <= 0F)
            {
                m_disolvingTimer = DISOLVING_TIME / 3F;
                ActualDamage();
            }

            if (m_parent == null)
            {
                Despawn(gameObject);
            }
        }
    }
    public void Damage()
    {
        ServerDamage();
    }
    [ServerRpc(RequireOwnership = false)]
    private void ServerDamage()
    {
        ObserverDamage();
    }
    [ObserversRpc]
    private void ObserverDamage()
    {
        if (IsOwner)
        {
            ActualDamage();
        }
    }
    private void ActualDamage()
    {
        health--;
        if (health <= 0)
        {
            Despawn(gameObject);
        }
    }
}
