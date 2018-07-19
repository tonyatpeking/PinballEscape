using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public ColorSet m_colorSet = ColorSet.BLUE;
    public List<DoorLight> m_lights;
    public SpriteRenderer m_lit;
    public SpriteRenderer m_unlit;

    public Transform m_slideToWayoint;
    public AnimationCurve m_slideCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float m_orangeSlideTime;
    public float m_orangeFadeTime;
    public float m_purpleSlideTime;
    public float m_purpleFadeTime;

    public float m_fadeInTime = 4;
    public float m_fullFadeWait = 3;
    public float m_fadeOutTime = 1;

    public float m_delayForSpamLights = 0.5f;
    public float m_spamLightDuration = 6.0f;
    public float m_delayForPurpleSlide = 6.0f;

    public AnimationCurve m_spamLightDelay;

    public DoorUnlockFade m_doorUnlockFade;

    private bool m_doorHasSlided = false;

    public AudioSource AS;
    public float AudioDelay = 0.25f;
    public AudioSource m_doorFadeBeep;
    public AnimationCurve m_pitchCurve;
    public AnimationCurve m_volumeCurve;

    public BallPauser m_ballPauser;

    float m_lightPreLightupTime = 0.2f;

    public void AddLight(DoorLight light)
    {
        m_lights.Add(light);
    }

    public void LightALightWithDelay()
    {
        DoorLight light = FindUnqueuedToBeLitLight();
        if (light != null)
        {
            light.SetLitWithDelay(ParticleSystemSpawner.GetInstance().GetPrototype().GetParticleLifeTime() - m_lightPreLightupTime);
        }
    }

    public DoorLight FindUnqueuedToBeLitLight()
    {
        foreach (DoorLight light in m_lights)
        {
            if (!light.IsQueuedToBeLit())
                return light;
        }
        return null;
    }

    // Use this for initialization
    void Start()
    {
        //doorTriggers = new GameObject[counter];
        m_lit = transform.Find("Lit").GetComponent<SpriteRenderer>();
        m_unlit = transform.Find("Unlit").GetComponent<SpriteRenderer>();
        m_doorUnlockFade = GameObject.FindGameObjectWithTag("DoorUnlockFade").GetComponent<DoorUnlockFade>();

        //AS = GetComponent<AudioSource>();

        m_ballPauser = GameObject.FindGameObjectWithTag("Player").GetComponent<BallPauser>();
        //if (m_colorSet == ColorSet.PURPLE)
        //    PlayPurpleDoorUnlockAnim();
    }

    int NumOfLitDoors()
    {
        int numLitDoors = 0;
        foreach (DoorLight light in m_lights)
        {
            if (light.IsLit())
            {
                ++numLitDoors;
            }
        }
        return numLitDoors;
    }

    // Update is called once per frame
    void Update()
    {
        if (NumOfLitDoors() >= m_lights.Count && !m_doorHasSlided)
        {
            ScoreController.GetInstance().AddDoorScore(m_colorSet);
            if (m_colorSet == ColorSet.PURPLE)
                PlayPurpleDoorUnlockAnim();
            else
                SlideOrangeDoor();
            m_doorHasSlided = true;
            //m_doorIsDestroyed = true;
        }
    }

    void PlayPurpleDoorUnlockAnim()
    {
        StartCoroutine(PurpleDoorUnlockAnim());
    }

    void SlideOrangeDoor()
    {
        StartCoroutine(SlideAndFadeSequence(m_orangeSlideTime, m_orangeFadeTime));
        AS.PlayDelayed(AudioDelay);

    }

    public static DoorController GetClosestDoorOfColor(ColorSet colorSet, Vector3 position)
    {
        float closestDistSqr = Mathf.Infinity;
        DoorController closestDoor = null;
        DoorController currentDoor = null;
        foreach (GameObject doorGo in GameObject.FindGameObjectsWithTag("Door"))
        {
            currentDoor = doorGo.GetComponent<DoorController>();

            if (currentDoor.m_colorSet != colorSet)
                continue;

            float sqrMagnitude = (position - doorGo.transform.position).sqrMagnitude;
            if (sqrMagnitude < closestDistSqr)
            {
                closestDoor = currentDoor;
                closestDistSqr = sqrMagnitude;
            }
        }
        return closestDoor;
    }

    IEnumerator PurpleDoorUnlockAnim()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        int savedLitOrder = m_lit.sortingOrder;
        int savedUnlitOrder = m_unlit.sortingOrder;
        int overlayOrder = m_doorUnlockFade.GetSortingOrder();

        foreach (DoorLight light in m_lights)
        {
            light.GetLitSprite().sortingOrder = overlayOrder + 2;
            light.GetUnlitSprite().sortingOrder = overlayOrder + 1;
        }

        m_lit.sortingOrder = overlayOrder + 2;
        m_unlit.sortingOrder = overlayOrder + 1;
        m_doorHasSlided = true;



        m_doorUnlockFade.StartFadeAnim(m_fadeInTime, m_fullFadeWait, m_fadeOutTime);
        m_ballPauser.SetBallPaused(true);

        yield return new WaitForSecondsRealtime(m_delayForSpamLights);
        StartCoroutine(SpamLightsAnim(m_spamLightDuration));

        yield return new WaitForSecondsRealtime(m_delayForPurpleSlide);
        AS.Play();
        StartCoroutine(SlideAndFadeSequence(m_purpleSlideTime, m_purpleFadeTime));

        yield return new WaitForSecondsRealtime(m_purpleSlideTime);
        m_ballPauser.SetBallPaused(false);

        yield return new WaitForSecondsRealtime(m_purpleFadeTime + 1.5f);

        m_lit.sortingOrder = savedLitOrder;
        m_unlit.sortingOrder = savedUnlitOrder;

        foreach (DoorLight light in m_lights)
        {
            light.GetLitSprite().sortingOrder = savedLitOrder;
            light.GetUnlitSprite().sortingOrder = savedUnlitOrder;
        }
    }

    IEnumerator SpamLightsAnim(float duration)
    {
        foreach (DoorLight light in m_lights)
        {
            light.SetIsFading(true);
        }

        float startTime = Time.realtimeSinceStartup;
        float delayTime = m_spamLightDelay.Evaluate(0);


        while (Time.realtimeSinceStartup - startTime < duration)
        {
            foreach (DoorLight light in m_lights)
            {
                float factorInAnim = (Time.realtimeSinceStartup - startTime) / duration;
                light.SetLitWithoutSound(true);
                m_doorFadeBeep.Play();
                m_doorFadeBeep.volume = m_volumeCurve.Evaluate(factorInAnim);
                m_doorFadeBeep.pitch = m_pitchCurve.Evaluate(factorInAnim);
                yield return new WaitForSeconds(delayTime);
                delayTime = m_spamLightDelay.Evaluate(factorInAnim);
            }

            if (delayTime < 0.01f)
                break;
        }

        foreach (DoorLight light in m_lights)
        {
            light.SetIsFading(false);
            light.SetLitWithoutSound(true);
        }
    }

    IEnumerator SlideAndFadeSequence(float slideTime, float fadeTime)
    {
        float timeSinceAnimStart = 0f;
        Vector3 originalPosition = transform.position;
        Vector3 finalPosition = m_slideToWayoint.position;

        float lerpFactor = 0f;

        while (lerpFactor < 1f)
        {
            lerpFactor = timeSinceAnimStart / slideTime;
            lerpFactor = m_slideCurve.Evaluate(lerpFactor);
            transform.position = Vector3.Lerp(originalPosition, finalPosition, lerpFactor);
            timeSinceAnimStart += Time.deltaTime;
            yield return null;
        }

        GetComponent<BoxCollider2D>().enabled = false;

        timeSinceAnimStart = 0f;
        lerpFactor = 0f;
        while (lerpFactor < 1f)
        {
            lerpFactor = timeSinceAnimStart / fadeTime;
            float alpha = Mathf.Lerp(1f, 0f, lerpFactor);
            Color color = new Color(1, 1, 1, alpha);
            m_lit.color = color;
            m_unlit.color = color;
            timeSinceAnimStart += Time.deltaTime;
            yield return null;
        }

        m_lit.enabled = false;
        m_unlit.enabled = false;
    }
}
