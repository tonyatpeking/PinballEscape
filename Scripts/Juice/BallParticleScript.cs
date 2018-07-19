using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallParticleScript : MonoBehaviour
{
    public AnimationCurve m_emitIntervalOnSpeed;
    public bool m_testEmit = false;
    public ParticleSystem m_trailPs;
    Rigidbody2D rb;
    public float m_trailEmitInterval = 0.1f;
    float m_timeOfLastEmit = 0f;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void EmitTrail(int numToEmit)
    {
        var emitParams = new ParticleSystem.EmitParams();

        emitParams.velocity = 2f * -rb.velocity;
        //float angle = Vector2.Angle(rb.velocity, new Vector2(1, 0));
        //var shape = m_bouncePs.shape;
        //shape.rotation = new Vector3(0, 0, angle);
        m_trailPs.Emit(emitParams, numToEmit);
    }



    // Update is called once per frame
    void Update()
    {
        m_trailEmitInterval = m_emitIntervalOnSpeed.Evaluate(rb.velocity.magnitude);
        if ( Time.time - m_timeOfLastEmit > m_trailEmitInterval)
        {
            m_timeOfLastEmit = Time.time;
            EmitTrail(2);
        }
        if(m_testEmit)
        {
            m_testEmit = false;
            EmitParticles();
        }
    }

    public void EmitParticles()
    {
        m_trailPs.Emit(20);
    }
}
