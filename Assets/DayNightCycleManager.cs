using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycleManager : NetworkBehaviour
{
    private static float MAX_TIME = 250F;

    private static float START_TIME = 20F;

    private static float NIGHT_TIME = 125F;

    [SerializeField] private GameObject m_previousLight;

    private Light m_light;

    private bool m_day;

    private float m_time;

    void Start()
    {
        m_time = START_TIME;
        m_day = START_TIME < NIGHT_TIME;
        m_light = GetComponent<Light>();
        m_light.bounceIntensity = m_day ? 3F : 0F;
        RenderSettings.reflectionIntensity = m_day ? 1.4F : 0F;
        RenderSettings.ambientIntensity = m_day ? 0.5F : 0F;
        m_light.intensity = m_day ? 5F : 0F;
        m_previousLight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime * 2F;
        if (m_time > NIGHT_TIME)
        {
            m_time += Time.deltaTime * 2F;
            if (m_day)
            {
                m_day = false;
                RenderSettings.reflectionIntensity = 0F;
                RenderSettings.ambientIntensity = 0F;
                m_light.intensity = 0F;
            }
        }
        if (m_time > MAX_TIME)
        {
            m_time -= MAX_TIME;
            m_day = true;
            RenderSettings.reflectionIntensity = 1.4F;
            RenderSettings.ambientIntensity = 0.5F;
            m_light.intensity = 5F;
        }
        transform.localEulerAngles = new Vector3(m_time / MAX_TIME * 360F, 30F, 0F);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        RequestTimeUpdate();
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
    }

    private void OnEnable()
    {
        m_previousLight.SetActive(false);
    }

    private void OnDisable()
    {
        m_previousLight.SetActive(true);
    }

    private void OnDestroy()
    {
        m_previousLight.SetActive(true);
    }
}
