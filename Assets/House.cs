using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] private Renderer[] m_windows;

    [SerializeField] private Material m_dayWindows;
    [SerializeField] private Material m_nightWindows;

    private bool m_day;
    public void UpdateDay(bool day)
    {
        m_day = day;
        for (int i = 0; i < m_windows.Length; i++)
        {
            m_windows[i].material = m_day ? m_dayWindows : m_nightWindows;
        }
    }
}
