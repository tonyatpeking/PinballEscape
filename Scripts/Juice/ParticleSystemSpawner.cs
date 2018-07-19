using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemSpawner : MonoSingleton<ParticleSystemSpawner>
{
    public ParticleController m_particleControllerToClone;

    public ParticleController GetPrototype()
    {
        return m_particleControllerToClone;
    }

    public ParticleController SpawnParticleController()
    {
        return Instantiate(m_particleControllerToClone) as ParticleController;
    }

    public void EmitAndForget(Vector3 startPos, Vector3 endPos, Color col, int particleCount)
    {
        SpawnParticleController().EmitAndForget( startPos,  endPos,  col, particleCount);
    }

}
