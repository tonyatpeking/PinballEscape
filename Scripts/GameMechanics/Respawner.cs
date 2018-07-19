using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Respawner : MonoBehaviour
{
    GameObject m_respawnPoint;

    void Awake()
    {
        m_respawnPoint = GameObject.FindWithTag("Respawn");
    }


    void Update()
    {
        if (!Application.isPlaying)
        {
            SnapToRespawnPoint();
        }
    }

    void SnapToRespawnPoint()
    {
        if( m_respawnPoint )
            transform.position = m_respawnPoint.transform.position;
    }
}
