using FishNet.Component.Spawning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private PlayerSpawner m_playerSpawner;

    [SerializeField] private Transform[] m_spawns;

    private void Awake()
    {
        Singleton = this;
    }

    void Start()
    {
        m_playerSpawner.Spawns = m_spawns;
    }

    private static SpawnManager m_singleton;
    public static SpawnManager Singleton
    {
        get => m_singleton;
        private set
        {
            if (m_singleton == null)
            {
                m_singleton = value;
            }
            else if (m_singleton != value)
            {
                Destroy(value);
            }
        }
    }

    public Transform GetRandomSpawn()
    {
        return m_spawns[Random.Range(0, m_spawns.Length)];
    }
}
