using FishNet;
using FishNet.Managing;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilding : NetworkBehaviour
{
    [SerializeField] private GameObject m_boxPrefab;
    private PlayerView m_playerView;
    void Start()
    {
        Vector3 size = m_boxPrefab.GetComponent<BoxCollider>().size;
        m_playerView = GetComponent<PlayerView>();
    }

    void Update()
    {
        if (IsOwner && Input.GetKeyDown(KeyCode.E))
        {
            TrySpawnBox();
        }
    }

    private void TrySpawnBox()
    {
        Vector3 direction = m_playerView.GetDirection();

        if(m_playerView.TryRaycastFromHead(out RaycastHit hit, 12F))
        {
            Vector3 position = hit.point - direction * 0.5F;
            GameObject newBox = Instantiate(m_boxPrefab, position, Quaternion.Euler(0F, transform.eulerAngles.y, 0F));
            InstanceFinder.ServerManager.Spawn(newBox, LocalConnection);
        }
    }
}
