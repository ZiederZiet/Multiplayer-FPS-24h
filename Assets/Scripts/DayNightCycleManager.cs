using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycleManager : NetworkBehaviour
{
    private static float MAX_TIME = 120F;

    private static float START_TIME = 20F;

    private static float NIGHT_TIME = MAX_TIME / 2F;

    [SerializeField] private GameObject m_previousLight;

    private Light m_light;

    private bool m_day;

    private float m_time;

    void Start()
    {
        m_light = GetComponent<Light>();
        AtStart();
    }

    void Update()
    {
        m_time += Time.deltaTime;
        if (m_time > NIGHT_TIME)
        {
            if (m_day)
            {
                m_day = false;
                RefreshSettings();
            }
        }
        if (m_time > MAX_TIME)
        {
            m_time -= MAX_TIME;
            m_day = true;
            RefreshSettings();
        }
        UpdatePosition();
    }

    private void RefreshSettings()
    {
        RenderSettings.reflectionIntensity = m_day ? 0.6F : 0F;
        RenderSettings.ambientIntensity = m_day ? 0.5F : 0F;
        m_light.intensity = m_day ? 5F : 0F;
    }

    private void UpdatePosition()
    {
        transform.localEulerAngles = new Vector3(m_time / MAX_TIME * 360F, 30F, 0F);
    }

    private void AtStart()
    {
        m_time = START_TIME;
        m_day = START_TIME < NIGHT_TIME;
        m_light.bounceIntensity = m_day ? 3F : 0F;
        UpdatePosition();
        RefreshSettings();
    }

    public override void OnStartClient()
    {
        RequestTimeUpdate();
        m_previousLight.SetActive(false);
    }

    [ServerRpc]
    public void RequestTimeUpdate()
    {
        TimeUpdate(m_time);
    }

    [ObserversRpc]
    public void TimeUpdate(float time)
    {
        m_time = time;
        UpdatePosition();
    }


    private void OnEnable()
    {
        Start();
    }

    private void OnDisable()
    {
        AtStart();
    }
}