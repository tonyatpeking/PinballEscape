using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoSingleton<ScreenFader>
{
    public Image m_blackImage;
    Color TransparentBlack = new Color(0, 0, 0, 0);
    Color SolidBlack = new Color(0, 0, 0, 1);
    private void Start()
    {
        m_blackImage.gameObject.SetActive(false);
    }

    public void FadeToBlack(float t = 1f, Action actionAtEnd = null)
    {
        m_blackImage.gameObject.SetActive(true);
        StartCoroutine(LerpColorOverTime(TransparentBlack, SolidBlack, t, actionAtEnd));
    }

    public void FadeToTransparent(float t = 1f, Action actionAtEnd = null)
    {
        m_blackImage.gameObject.SetActive(true);
        StartCoroutine(LerpColorOverTime(SolidBlack, TransparentBlack, t, actionAtEnd));
        // Disable gameobject for better performance
        StartCoroutine(DelayedAction(t, delegate { m_blackImage.gameObject.SetActive(false); }));
    }

    IEnumerator LerpColorOverTime( Color start, Color end, float lengthOfTime, Action actionAtEnd)
    {
        float timeElasped = 0f;

        while( timeElasped < lengthOfTime )
        {
            timeElasped += Time.deltaTime;
            m_blackImage.color = Color.Lerp(start, end, timeElasped / lengthOfTime);
            yield return null;
        }
        if ( actionAtEnd != null )
            actionAtEnd();
    }

    IEnumerator DelayedAction(float lengthOfTime, Action actionAtEnd)
    {
        yield return new WaitForSeconds(lengthOfTime);
        actionAtEnd();
    }
}
