using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{

    public ParticleSystem.Particle[] m_particles;
    public ParticleSystem m_ps;
    public AnimationCurve m_driftCurve;

    public float m_delayBeforeDrift = 0.5f;
    public float m_driftDuration = 1f;


    public float GetParticleLifeTime()
    {
        return m_delayBeforeDrift + m_driftDuration;
    }

    public void EmitAndForget(Vector3 startPos, Vector3 endPos, Color col, int particleCount)
    {
        if (!m_ps)
            return;
        var main = m_ps.main;
        m_ps.Stop();
        main.maxParticles = particleCount;
        main.startColor = col;
        main.startLifetime = GetParticleLifeTime();
        main.duration = GetParticleLifeTime();
        m_ps.Play();
        m_particles = new ParticleSystem.Particle[m_ps.main.maxParticles];

        transform.position = startPos;
        StartCoroutine(BurstAndDrift(endPos, particleCount));
    }

    IEnumerator BurstAndDrift(Vector3 driftPosition, int particleCount)
    {
        m_ps.Emit(particleCount);

        yield return new WaitForSeconds(m_delayBeforeDrift);

        float timeOfBurst = Time.time;
        float timeSinceBurst = 0;

        int numParticles = m_ps.GetParticles(m_particles);
        Vector3[] savedPos = new Vector3[numParticles];
        for (int i = 0; i < numParticles; ++i)
        {
            savedPos[i] = m_particles[i].position;
        }

        while (timeSinceBurst < m_driftDuration)
        {
            m_ps.GetParticles(m_particles);
            timeSinceBurst = Time.time - timeOfBurst;
            float lerpFactor = timeSinceBurst / m_driftDuration;
            lerpFactor = m_driftCurve.Evaluate(lerpFactor);
            for (int i = 0; i < numParticles; i++)
            {
                m_particles[i].position = Vector3.Lerp(savedPos[i], driftPosition, lerpFactor);  //Vector3.MoveTowards(m_particles[i].position, driftPosition, Time.deltaTime * 50f);
            }
            m_ps.SetParticles(m_particles, numParticles);
            yield return null;
        }
        m_ps.Clear();
        Destroy(gameObject);
    }
}
