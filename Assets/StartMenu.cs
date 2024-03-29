using FishNet.Managing;
using FishNet.Transporting.Tugboat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_menu;
    [SerializeField] private GameObject m_hostMenu;

    [SerializeField] private TMPro.TMP_InputField m_serverIpInputField;

    private NetworkManager m_networkManager;
    private Tugboat m_tugboat;

    void Start()
    {
        m_networkManager = FindObjectOfType<NetworkManager>();
        m_tugboat = FindObjectOfType<Tugboat>();
    }

    public void ConnectClientButton()
    {
        m_tugboat.SetClientAddress(m_serverIpInputField.text);
        m_networkManager.ClientManager.StartConnection();
        if (!m_menu.activeSelf)
        {
            m_hostMenu.SetActive(false);
        }
        m_menu.SetActive(false);
    }

    public void ServerButton()
    {
        m_networkManager.ServerManager.StartConnection();
        m_menu.SetActive(false);
        m_tugboat.SetClientAddress("127.0.0.1");
        m_hostMenu.SetActive(true);
    }
}
