using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text m_ammoText;

    [SerializeField] private TMPro.TMP_Text m_fps;

    [SerializeField] private GameObject m_hitMarker;
    [SerializeField] private AudioSource m_hitMarkSound;

    private float m_hitMarkTimer;

    private static HudManager m_singleton;
    public static HudManager Singleton
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

    private void Awake()
    {
        Singleton = this;
    }

    public TMPro.TMP_Text GetAmmoTextMesh()
    {
        return m_ammoText;
    }

    public void HitMark()
    {
        m_hitMarkTimer = 0.2F;
        m_hitMarker.SetActive(true);
        m_hitMarkSound.Stop();
        m_hitMarkSound.Play();
    }

    void Start()
    {
        m_hitMarker.SetActive(false);
    }
    void Update()
    {
        if (m_hitMarkTimer > 0F)
        {
            m_hitMarkTimer -= Time.deltaTime;
            if (m_hitMarkTimer <= 0f)
            {
                m_hitMarker.SetActive(false);
            }
        }

        m_fps.text = (1F / Time.deltaTime).ToString("##.###");
    }
}
