using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherController : MonoBehaviour
{
    public Vector2 m_launchVelocity = new Vector2(0, 100);

    public AudioSource AS;

    public bool m_isBallHooked = true;
    public float m_turboSeconds = 1f;

    GameObject m_ballHook;
    Rigidbody2D m_ballRB;

    void Awake()
    {
        m_ballHook = transform.Find("BallHook").gameObject;
        m_ballRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        AS = GetComponent<AudioSource>(); 
    }

    public bool IsBallHooked()
    {
        return m_isBallHooked;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isBallHooked)
        {
            SnapBallToHook();
        }
        if (m_isBallHooked && WasFingerJustReleased())
        {
            LaunchBall();
        }
    }

    void SnapBallToHook()
    {
        m_ballRB.velocity = Vector2.zero;
        m_ballRB.angularVelocity = 0;
        m_ballRB.transform.position = m_ballHook.transform.position;
    }

    void LaunchBall()
    {
        m_isBallHooked = false;
        m_ballRB.velocity = m_launchVelocity;
        m_ballRB.GetComponent<ClampSpeed>().TurboForSeconds(m_turboSeconds, m_launchVelocity.magnitude);
    }

    bool WasFingerJustReleased()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            AS.Play(); 

            return true;
        }

        if(Input.GetMouseButtonUp(0))
        {
            AS.Play();

            return true;
        }

        return false;
    }

}
