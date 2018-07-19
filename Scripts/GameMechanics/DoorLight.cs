using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLight : MonoBehaviour {

    public ColorSet m_colorSet = ColorSet.BLUE;

    SpriteRenderer m_litSprite;
    SpriteRenderer m_unlitSprite;

    float m_timeOfLit = 0f;
    public float m_glowInterval = 0.7f;
    public float m_glowMinAlpha = 0.85f;
    public float m_glowMaxAlpha = 1f;

    bool m_isLit = false;
    bool m_queuedToBeLit = false;

    bool m_isFading = false;
    float m_fadePerSecond = 3f;

    public AudioSource AS;
    float m_audioDelayTime = 0f;
    // Use this for initialization
    void Start () {
        m_litSprite = transform.Find("Lit").GetComponent<SpriteRenderer>();
        m_unlitSprite = transform.Find("Unlit").GetComponent<SpriteRenderer>();
        m_litSprite.enabled = false;
        m_unlitSprite.enabled = true;
        DoorController door = GetClosestDoor();
        if( door )
            GetClosestDoor().AddLight(this);

        AS = GetComponent<AudioSource>();
    }

    public SpriteRenderer GetLitSprite()
    {
        return m_litSprite;
    }

    public SpriteRenderer GetUnlitSprite()
    {
        return m_unlitSprite;
    }

    private void Update()
    {
        if(m_isLit && !m_isFading)
        {
            Glow();
        }

        if( m_isFading )
        {
            Fade();
        }
    }

    void Fade()
    {
        Color litColor = m_litSprite.color;
        litColor.a = litColor.a - Time.deltaTime * m_fadePerSecond;
        m_litSprite.color = litColor;
    }

    public void SetIsFading( bool isFading )
    {
        m_isFading = isFading;
    }

    void Glow()
    {
        float t = Mathf.PingPong(Time.time - m_timeOfLit, m_glowInterval);
        t = t / m_glowInterval;
        float litAlpha = Mathf.SmoothStep(m_glowMinAlpha, m_glowMaxAlpha, t);

        Color litColor = m_litSprite.color;
        m_litSprite.color = litColor;

        //Color unlitColor = m_unlitSprite.color;
        //unlitColor.a = 1 - litColor.a;
        //m_unlitSprite.color = unlitColor;
    }

    public void SetLitWithoutSound(bool isLit)
    {
        m_isLit = isLit;
        m_litSprite.enabled = true;

        Color litColor = m_litSprite.color;
        litColor.a = 1.0f;
        m_litSprite.color = litColor;
    }

    public void SetLitWithSound( bool isLit)
    {
        SetLitWithoutSound(isLit);

        //Mihir Addons
        AS.Play();
        //AS.PlayDelayed(m_audioDelayTime);
    }

    public void SetLitWithDelay( float delay )
    {
        StartCoroutine(LightWithDelay(delay));
    }

    IEnumerator LightWithDelay( float delay )
    {
        m_queuedToBeLit = true;
        yield return new WaitForSeconds(delay);
        SetLitWithSound(true);
    }

    public bool IsQueuedToBeLit()
    {
        return m_queuedToBeLit;
    }

    public bool IsLit()
    {
        return m_isLit;
    }

    DoorController GetClosestDoor()
    {
        return DoorController.GetClosestDoorOfColor(m_colorSet, transform.position);
    }
}
