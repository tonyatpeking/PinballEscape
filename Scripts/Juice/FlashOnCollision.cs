using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashOnCollision : MonoBehaviour
{
    public ColorSet m_colorSet = ColorSet.BLUE;
    public AnimationCurve curve;

    float m_timeAnimationStarted;
    float m_animationLength;
    SpriteRenderer m_litSprite;
    SpriteRenderer m_unlitSprite;
    bool m_isPlaying = false;

    float m_timeOfForcedLit = 0f;
    bool m_isForcedLit = false;
    float m_forcedLitAlpha = 0f;

    public float m_glowInterval = 0.7f;
    public float m_glowMinAlpha = 0.85f;
    public float m_glowMaxAlpha = 1f;

    public void SetForcedLit( bool isLit )
    {
        m_timeOfForcedLit = Random.Range(0, m_glowInterval);
        m_isForcedLit = isLit;
    }

    void Glow()
    {
        float t = Mathf.PingPong(Time.time - m_timeOfForcedLit, m_glowInterval);
        t = t / m_glowInterval;
        m_forcedLitAlpha = Mathf.SmoothStep(m_glowMinAlpha, m_glowMaxAlpha, t);

        Color litColor = m_litSprite.color;
        litColor.a = m_forcedLitAlpha;
        m_litSprite.color = litColor;

        //Color unlitColor = m_unlitSprite.color;
        //unlitColor.a = 1 - litColor.a;
        //m_unlitSprite.color = unlitColor;
    }

    private void Awake()
    {
        m_litSprite = transform.Find("Lit").GetComponent<SpriteRenderer>();
        m_unlitSprite = transform.Find("Unlit").GetComponent<SpriteRenderer>();
        m_litSprite.enabled = false;
        m_unlitSprite.enabled = true;
        if (curve.length == 0)
        {
            m_animationLength = 0;
        }
        else
        {
            m_animationLength = curve.keys[curve.length - 1].time;
        }
    }

    private void Update()
    {
        if( m_isForcedLit )
        {
            Glow();
        }

        if (m_isPlaying)
        {
            float timeSinceAnimStart = Time.time - m_timeAnimationStarted;
            if (timeSinceAnimStart > m_animationLength)
            {
                SetIsPlaying(false);

                return;
            }
            float blend = curve.Evaluate(timeSinceAnimStart);
            Color litColor = m_litSprite.color;
            litColor.a = Mathf.Max( blend, m_forcedLitAlpha );
            m_litSprite.color = litColor;

            //Color unlitColor = m_unlitSprite.color;
            //unlitColor.a = 1 - litColor.a;
            //m_unlitSprite.color = unlitColor;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_timeAnimationStarted = Time.time;
            SetIsPlaying(true);
            ScoreController.GetInstance().AddBumperSlingshotScore(m_colorSet);
        }
    }

    void SetIsPlaying(bool isPlaying)
    {
        m_isPlaying = isPlaying;
        if (m_isPlaying)
        {
            m_litSprite.enabled = true;
        }
        else
        {
            if( !m_isForcedLit )
            {
                m_litSprite.enabled = false;
                //Color unlitColor = m_unlitSprite.color;
                //unlitColor.a = 1;
                //m_unlitSprite.color = unlitColor;
            }
        }
    }
}
