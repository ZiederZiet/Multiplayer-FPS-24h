using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private GameObject[] m_nonLocalOnly;
    [SerializeField] private Transform m_head;
    [SerializeField] private Renderer m_renderer;
    private Transform m_camera;
    void Start()
    {
        m_camera = Camera.main.transform;
    }
    void Update()
    {
        if (IsOwner)
        {
            m_camera.position = m_head.position;
            m_camera.rotation = m_head.rotation;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        for (int i = 0; i < m_nonLocalOnly.Length; i++)
        {
            m_nonLocalOnly[i].SetActive(IsOwner);
        }
        if (IsOwner)
        {
            m_renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }
}
