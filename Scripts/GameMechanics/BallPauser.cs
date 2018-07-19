using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallPauser : MonoBehaviour
{
    public CustomTrampoline m_trampoline;
    Rigidbody2D m_ballRB;
    bool m_isBallPaused = false;

    bool m_hasSavedValues = false;

    Vector2 m_velocity;
    float m_gravity;
    float m_drag;

    // Use this for initialization
    void Awake()
    {
        m_ballRB = GetComponent<Rigidbody2D>();
    }

    public void SetBallPaused( bool ballPaused )
    {
        m_isBallPaused = ballPaused;
        if ( ballPaused)
        {
            m_velocity = m_ballRB.velocity;
            m_gravity = m_ballRB.gravityScale;
            m_drag = m_ballRB.drag;
            //m_ballRB.velocity = Vector2.zero;
            m_ballRB.drag = 7;
            m_ballRB.gravityScale = 0;
            m_hasSavedValues = true;

            m_trampoline.SetDisabled(true);
        }
        else
        {
            if(m_hasSavedValues)
            {
                m_ballRB.velocity = m_velocity;
                m_ballRB.gravityScale = m_gravity;
                m_ballRB.drag = m_drag;
                m_hasSavedValues = false;
            }

            m_trampoline.SetDisabled(false);
        }
    }
}
