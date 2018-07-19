using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTutorial : MonoBehaviour
{
    public LauncherController m_launcher;
    public BallPauser m_ballPauser;
    bool m_tutorialHasStarted = false;

    public GameObject m_touch1;
    public GameObject m_touch2;
    public GameObject m_tutorialPoint1;
    public GameObject m_tutorialPoint2;

    public float m_delayBeforeTutorialStarts = 0.5f;
    public float m_delayBeforeGameResumes = 0.3f;

    public float m_tutorialDistanceCheck = 2;

    SpriteRenderer m_renderer;
    Animator m_animator;

    // Use this for initialization
    void Start()
    {
        m_launcher = GameObject.FindGameObjectWithTag("Launcher").GetComponent<LauncherController>();
        m_ballPauser = GameObject.FindGameObjectWithTag("Player").GetComponent<BallPauser>();
        GameObject[] touches = GameObject.FindGameObjectsWithTag("TouchPoint");
        m_touch1 = touches[0];
        m_touch2 = touches[1];
        m_renderer = GetComponent<SpriteRenderer>();
        m_renderer.enabled = false;
        m_animator = GetComponent<Animator>();
        m_animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_tutorialHasStarted)
            return;

        if (m_launcher.m_isBallHooked)
            return;

        StartTutorial();
    }

    void StartTutorial()
    {

        m_tutorialHasStarted = true;
        StartCoroutine(TutorialSequence());
    }

    IEnumerator TutorialSequence()
    {
        yield return new WaitForSeconds(m_delayBeforeTutorialStarts);

        m_ballPauser.SetBallPaused(true);

        yield return new WaitForSeconds(m_delayBeforeTutorialStarts);
        m_renderer.enabled = true;
        m_animator.enabled = true;

        while (!PlayerPassTutorialCheck())
            yield return null;

        yield return new WaitForSeconds(m_delayBeforeGameResumes);

        m_ballPauser.SetBallPaused(false);
    }

    bool PlayerPassTutorialCheck()
    {
        Vector2 touch1Pos = m_touch1.transform.position;
        Vector2 touch2Pos = m_touch2.transform.position;
        Vector2 tutorialPos1 = m_tutorialPoint1.transform.position;
        Vector2 tutorialPos2 = m_tutorialPoint2.transform.position;
        if ((CloseEnough(touch1Pos, tutorialPos1) && CloseEnough(touch2Pos, tutorialPos2))
            || (CloseEnough(touch1Pos, tutorialPos2) && CloseEnough(touch2Pos, tutorialPos1)))
        {
            Debug.Log("pass tutorial");
            m_renderer.enabled = false;
            m_animator.enabled = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CloseEnough(Vector2 a, Vector2 b)
    {
        return (a - b).sqrMagnitude < m_tutorialDistanceCheck * m_tutorialDistanceCheck;
    }



}
