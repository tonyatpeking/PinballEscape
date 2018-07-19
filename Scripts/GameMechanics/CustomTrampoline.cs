using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTrampoline : MonoBehaviour
{
    public GameObject m_fingerStart;
    public GameObject m_fingerEnd;
    public GameObject m_ball;
    public GameObject m_trampoline;
    public BoxCollider2D m_trampolineCollider;
    public AnimationCurve m_speedCurve;

    public bool m_useCorrectPhysics = true;

    public GameObject[] m_noDrawZones;

    //Vector3 m_trampolineStart;
    //Vector3 m_trampolineEnd;

    //Vector3 m_prevBallPos;
    //Vector3 m_prevTrampolineEndPos;
    bool m_trampolineWasUsed = true;

    public bool m_disabled = false;

    public TrampolineStretchy m_stretchy;

    //bool m_wasBallProjectedOnTrampoline = false;
    public void SetDisabled( bool disabled )
    {
        m_disabled = disabled;
        if( m_disabled)
            m_trampolineCollider.enabled = false;
        else
        {
            m_trampolineCollider.enabled = true;
        }
    }

    //MathUtils.Side side = MathUtils.Side.Left;

    public void SetTrampolineUsed(bool wasUsed)
    {
        m_trampolineWasUsed = wasUsed;
        //SpriteRenderer renderer = m_trampoline.GetComponent<SpriteRenderer>();
        //SpriteRenderer fingerStartRenderer = m_fingerStart.GetComponentInChildren<SpriteRenderer>();
        //SpriteRenderer fingerEndRenderer = m_fingerEnd.GetComponentInChildren<SpriteRenderer>();
        //Color newColor = renderer.color;
        if (wasUsed)
        {
            //newColor.a = 0.1f;
            //renderer.color = newColor;
            //fingerStartRenderer.color = newColor;
            //fingerEndRenderer.color = newColor;
            m_stretchy.FlashStretchFade();
            m_trampolineCollider.enabled = false;
        }
        if (!wasUsed)
        {
            m_stretchy.SetUnfade();
            //newColor.a = 1f;
            //renderer.color = newColor;
            //fingerStartRenderer.color = newColor;
            //fingerEndRenderer.color = newColor;
            if(!m_disabled)
                m_trampolineCollider.enabled = true;
        }
    }

    private void Start()
    {
        m_noDrawZones = GameObject.FindGameObjectsWithTag("NoDrawZone");
        m_stretchy = GetComponent<TrampolineStretchy>();

    }

    //public bool IsBallInNoDrawZone()
    //{
    //    foreach( GameObject noDrawZone in m_noDrawZones )
    //    {
    //        if (noDrawZone.GetComponent<SpriteRenderer>().bounds.Contains(m_ball.transform.position))
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.tag == "Player" )
        {
            SetTrampolineUsed(true);
        }
        //collision.contacts


        ContactPoint2D contact = collision.contacts[0];

        Vector2 velocity = collision.rigidbody.velocity;
        Vector2 normal = contact.normal.normalized;
        Vector2 tangent = normal.GetOrthogonalVector();
        Vector2 velocityOnNormal = normal * Vector2.Dot(normal, velocity);
        Vector2 velocityOnTangent = tangent * Vector2.Dot(tangent, velocity);
        float speed = m_speedCurve.Evaluate(velocityOnNormal.magnitude);
        Vector2 newVelocity = velocityOnTangent - normal * speed;
        collision.rigidbody.velocity = newVelocity;
        return;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    bool ballIsInNoDrawZone = IsBallInNoDrawZone();
    //    m_trampolineStart = m_fingerStart.transform.position;
    //    m_trampolineEnd = m_fingerEnd.transform.position;
    //    // trajectory = ballPos <-> ballPosLastFrame
    //    // if trajectory crosses with trampoline
    //    if ( MathUtils.DoLinesIntersect(
    //            m_ball.transform.position,
    //            m_prevBallPos,
    //            m_trampolineStart,
    //            m_trampolineEnd)
    //        || MathUtils.IsPointInTriangle( //ball is in the triangle formed by the trampoline start end and previous end
    //            m_ball.transform.position,
    //            m_trampolineStart,
    //            m_trampolineEnd,
    //            m_prevTrampolineEndPos)
    //        || MathUtils.IsPointInTriangle( //ball is in the triangle formed by the trampoline start end and previous end
    //            m_prevBallPos,
    //            m_trampolineStart,
    //            m_trampolineEnd,
    //            m_prevTrampolineEndPos)
    //        )
    //    {
    //        if (!m_trampolineWasUsed && !ballIsInNoDrawZone)
    //        {
    //            // flip ball velocity on trampoline normal axis
    //            Vector2 trampolineTangent = ( m_trampolineEnd - m_trampolineStart ).normalized;
    //            Vector2 trampolineNormal = trampolineTangent.GetOrthogonalVector().normalized;
    //            Rigidbody2D rb = m_ball.GetComponent<Rigidbody2D>();
    //            Vector2 newVelocity;
    //            if (m_useCorrectPhysics)
    //            {
    //                Vector2 velocityOnNormal = Vector2.Dot(rb.velocity, trampolineNormal) * trampolineNormal;
    //                newVelocity = rb.velocity - velocityOnNormal * (1 + m_speedCurve.Evaluate(velocityOnNormal.magnitude));
    //            }

    //            else
    //            {
    //                newVelocity = Vector2.Reflect(rb.velocity, trampolineNormal);
    //                float speed = rb.velocity.magnitude;
    //                newVelocity.Normalize();
    //                newVelocity *= m_speedCurve.Evaluate(speed);// add a velocity according to curve
    //            }


    //            rb.velocity = newVelocity;

    //            SetTrampolineUsed(true);
    //        }
    //    }


    //    m_prevTrampolineEndPos = m_trampolineEnd;
    //    m_prevBallPos = m_ball.transform.position;
    //}
}
