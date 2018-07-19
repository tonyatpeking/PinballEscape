using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTimeOnTouch : MonoBehaviour
{
    public float m_globalTimeScale = 0.1f;
    public float m_touchTimeScale = 1.0f;
    public bool m_isPaused = false;

    public void SetGlobalTimeScale(float timeScale)
    {
        if (Mathf.Abs(timeScale - m_globalTimeScale) < 0.0001f)
            return;

        m_globalTimeScale = timeScale;
    }

    public void SetTouchTimeScale(float timeScale)
    {
        m_touchTimeScale = timeScale;
    }

    public void SetIsPaused(bool isPaused)
    {
        m_isPaused = isPaused;
    }

    void Update()
    {
        TouchController touchController = GetComponent<TouchController>();
        if (m_isPaused)
        {
            SetTimeScale(0);
        }
        else
        {
            if (touchController.m_isAnyFingerTouching)
            {
                SetTimeScale(m_touchTimeScale);
            }
            else
            {
                SetTimeScale(m_globalTimeScale);
            }
        }

    }

    void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.02f * timeScale;
    }

    private void OnDestroy()
    {
        SetTimeScale(m_globalTimeScale);
    }
}
