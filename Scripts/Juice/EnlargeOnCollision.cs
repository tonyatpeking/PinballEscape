using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnlargeOnCollision : MonoBehaviour
{
    public AnimationCurve curve;

    float m_timeAnimationStarted;
    float m_animationLength;
    SpriteRenderer m_lit;
    SpriteRenderer m_unlit;
    bool m_isPlaying = false;

    private void Awake()
    {
        m_lit = transform.Find("Lit").GetComponent<SpriteRenderer>();
        m_unlit = transform.Find("Unlit").GetComponent<SpriteRenderer>();
        if(curve.length == 0)
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
        if( m_isPlaying )
        {
            float timeSinceAnimStart = Time.time - m_timeAnimationStarted;
            if( timeSinceAnimStart > m_animationLength )
            {
                m_isPlaying = false;
                return;
            }
            float scale = curve.Evaluate(timeSinceAnimStart);
            m_lit.transform.localScale = new Vector3(scale, scale, scale);
            m_unlit.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            m_timeAnimationStarted = Time.time;
            m_isPlaying = true;
        }
    }
}
