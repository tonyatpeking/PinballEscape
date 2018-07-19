using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampSpeed : MonoBehaviour
{
    public float m_normalMaxSpeed = 100;
    public float m_turboMaxSpeed = 10000;
    public float m_currentMaxSpeed = 0;

    public float m_turboStartTime = 0;
    public float m_turboDuration = 10000;
    public bool m_turboEnabled = true;
    Rigidbody2D rb;

    bool m_disableClamp = false;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DecayTurbo();
        if (!m_disableClamp)
        {
            Vector3 v = rb.velocity;
            if (v.sqrMagnitude > (m_currentMaxSpeed * m_currentMaxSpeed))
            {
                v.Normalize();
                v *= m_currentMaxSpeed;
                rb.velocity = v;
            }
        }

    }

    public void SetMaxSpeed(float value)
    {
        m_normalMaxSpeed = value;
        m_currentMaxSpeed = value;
    }

    public void TurboForSeconds(float duration, float turboMaxSpeed)
    {
        m_turboEnabled = true;
        m_turboMaxSpeed = turboMaxSpeed;
        m_turboStartTime = Time.time;
        m_turboDuration = duration;
    }

    void DecayTurbo()
    {
        if (m_turboEnabled)
        {
            float timeSinceTurboStart = Time.time - m_turboStartTime;
            float percentageInDuration = timeSinceTurboStart / m_turboDuration;
            percentageInDuration = Mathf.Clamp01(percentageInDuration);
            m_currentMaxSpeed = Mathf.Lerp(m_turboMaxSpeed, m_normalMaxSpeed, percentageInDuration);

            if (percentageInDuration == 1)
            {
                m_turboEnabled = false;
            }

        }
    }


    public void DisableForSeconds(float seconds)
    {
        m_disableClamp = true;
        StartCoroutine(EnableBallInSeconds(seconds));
    }

    IEnumerator EnableBallInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        m_disableClamp = false;
    }


}
