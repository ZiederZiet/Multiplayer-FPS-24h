using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private static float MAX_HEALTH;

    private float m_health;

    private void Start()
    {
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
        }
    }

    public void Die()
    {
        m_health = MAX_HEALTH;
        transform.position = SpawnManager.Singleton.GetRandomSpawn().position;
    }
}
