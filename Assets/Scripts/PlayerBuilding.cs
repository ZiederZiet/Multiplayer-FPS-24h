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

    private float m_couldown;

    private float m_boxes;

    void Start()
    {
        m_playerView = GetComponent<PlayerView>();
    }

    void Update()
    {
        if (IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.E) && m_boxes < 20F && m_couldown <= 0F)
            {
                TrySpawnBox();
            }
            if (m_boxes > 0F)
            {
                m_boxes -= Time.deltaTime / 12F;
            }
            if (m_boxes > 17F)
            {
                m_boxes -= Time.deltaTime / 12F;
            }
            if (m_couldown > 0F)
            {
                m_couldown -= Time.deltaTime;
            }
        }
    }

    private void TrySpawnBox()
    {
        Vector3 direction = m_playerView.GetDirection();

        if(m_playerView.TryRaycastFromHead(out RaycastHit hit, 12F))
        {
            Vector3 position = hit.point - direction;
            GameObject newBox = Instantiate(m_boxPrefab, position, Quaternion.Euler(0F, transform.eulerAngles.y, 0F));
            newBox.GetComponent<Box>().SetParent(hit.transform);
            InstanceFinder.ServerManager.Spawn(newBox, LocalConnection);
            m_boxes += 1F;
            m_couldown = 0.5F;
        }
    }
}
