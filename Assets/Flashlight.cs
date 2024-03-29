using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : NetworkBehaviour
{
    private static Vector3 SWITCH_EULER_OFF = new Vector3(287F, 270F, 90F);
    private static Vector3 SWITCH_EULER_ON = new Vector3(282F, 90F, 269F);

    private bool m_on;

    [SerializeField] private Transform m_switch;

    [SerializeField] private GameObject m_light;

    [SerializeField] private Renderer m_renderer;

    [SerializeField] private Material m_onMaterial;
    [SerializeField] private Material m_offMaterial;

    private Material[] m_materials;

    void Start()
    {
        m_materials = m_renderer.materials;
        m_on = true;
        UpdateOn();
    }
    
    public void TriggerSwitch()
    {
        ServerSwitchRpc(!m_on);
    }

    public void Switch(bool on)
    {
        m_on = on;
        UpdateOn();
    }

    [ServerRpc]
    public void ServerSwitchRpc(bool on)
    {
        m_on = on;
        ObserverSwitchRpc(on);
    }

    [ObserversRpc]
    public void ObserverSwitchRpc(bool on)
    {
        Switch(on);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ServerRequestInfoRpc(NetworkConnection conn)
    {
        TargetSwitchRpc(conn, m_on);
    }

    [TargetRpc]
    public void TargetSwitchRpc(NetworkConnection conn, bool on)
    {
        Switch(on);
    }

    void UpdateOn()
    {
        m_switch.localEulerAngles = m_on ? SWITCH_EULER_ON : SWITCH_EULER_OFF;
        m_light.SetActive(m_on);
        m_materials[1] = m_on ? m_onMaterial : m_offMaterial;
        m_renderer.materials = m_materials;
    }

    public void ResetSwitch()
    {
        if (!m_on)
        {
            TriggerSwitch();
        }
    }


    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner)
        {
            ServerRequestInfoRpc(LocalConnection);
        }
    }
}
