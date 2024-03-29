using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : NetworkBehaviour
{
    private static float DISOLVING_TIME = 120F;

    private float m_disolvingTimer;

    private int health;
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
