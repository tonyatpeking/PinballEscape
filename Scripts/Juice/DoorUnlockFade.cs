using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnlockFade : MonoBehaviour
{

    public SpriteRenderer m_overlaySprite;
    // Use this for initialization
    void Start()
    {
        //StartFadeAnim(4, 3, 1);
    }

    public void StartFadeAnim(float fadeInTime, float fullFadewait, float fadeOutTime)
    {
        StartCoroutine(FadeAnim(fadeInTime, fullFadewait, fadeOutTime));
    }

    public int GetSortingOrder()
    {
        return m_overlaySprite.sortingOrder;
    }

    void SetOverlayAlpha(float a)
    {
        Color color = m_overlaySprite.color;
        color.a = a;
        m_overlaySprite.color = color;
    }

    IEnumerator FadeAnim(float fadeInTime, float fullFadewait, float fadeOutTime)
    {
        m_overlaySprite.enabled = true;
        float timeOfStart = Time.realtimeSinceStartup;
        float timeInAnim = 0;
        float animFraction = 0;
        while (animFraction < 1f)
        {
            SetOverlayAlpha(animFraction);
            timeInAnim = Time.realtimeSinceStartup - timeOfStart;
            animFraction = timeInAnim / fadeInTime;
            yield return null;
        }

        SetOverlayAlpha(1f);
        yield return new WaitForSecondsRealtime(fullFadewait);

        timeOfStart = Time.realtimeSinceStartup;
        timeInAnim = 0;
        animFraction = 0;
        while (animFraction < 1f)
        {
            SetOverlayAlpha(1f - animFraction);
            timeInAnim = Time.realtimeSinceStartup - timeOfStart;
            animFraction = timeInAnim / fullFadewait;
            yield return null;
        }

        m_overlaySprite.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
