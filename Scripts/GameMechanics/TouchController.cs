using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TouchController : MonoBehaviour
{

    public TrampolineStretchy trampoline;
    public List<GameObject> balls;
    public Text consolText;
    public Camera mycamera;
    int m_controlScheme = 2;
    public bool m_isAnyFingerTouching = false;
    int m_currentDownTouchID = -1;
    Vector3 m_touchBeginWorldPos;
    public CustomTrampoline m_customTrampoline;

    LauncherController m_launcher;

    public void SetControlScheme(int controlSchemeNum)
    {
        m_controlScheme = controlSchemeNum;
    }

    void Start()
    {
        mycamera = Camera.main;
        GameObject launcherGO = GameObject.FindGameObjectWithTag("Launcher");
        if(launcherGO)
            m_launcher = launcherGO.GetComponent<LauncherController>();

    }

    void OnFirstFingerDown()
    {
        m_customTrampoline.SetTrampolineUsed(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_launcher)
            return;
        if (m_launcher.IsBallHooked())
            return;

        int fingerCount = 0;
        consolText.text = "";
        consolText.text += "\nControl Scheme: " + m_controlScheme;

        // multitouch input
        if (m_controlScheme == 1)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                {
                    Vector2 worldPos = mycamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, mycamera.nearClipPlane));
                    balls[fingerCount].transform.position = new Vector3(worldPos.x, worldPos.y, balls[fingerCount].transform.position.z);
                    fingerCount++;
                    consolText.text += "\nTouch:\n";
                    consolText.text += touch.position.ToString();
                    consolText.text += "\nWorld:\n";
                    consolText.text += worldPos.ToString();
                }
            }
            consolText.text += "\nTouch Count:\n";
            consolText.text += fingerCount.ToString();
        }

        // single touch drag input
        m_isAnyFingerTouching = false;
        if (m_controlScheme == 2 || m_controlScheme == 3)
        {
            consolText.text += "\nFinger Id:\n";
            consolText.text += m_currentDownTouchID.ToString();
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                    m_isAnyFingerTouching = true;
                if (touch.phase == TouchPhase.Began && m_currentDownTouchID == -1)
                {
                    m_currentDownTouchID = touch.fingerId;
                    Vector2 worldPos = mycamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, mycamera.nearClipPlane));
                    balls[0].transform.position = new Vector3(worldPos.x, worldPos.y, balls[0].transform.position.z);
                    m_touchBeginWorldPos = balls[0].transform.position;
                    OnFirstFingerDown();
                    balls[1].transform.position = balls[0].transform.position;
                    break;
                }
                if (touch.phase == TouchPhase.Moved && m_currentDownTouchID == touch.fingerId)
                {
                    Vector2 worldPos = mycamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, mycamera.nearClipPlane));
                    balls[1].transform.position = new Vector3(worldPos.x, worldPos.y, balls[1].transform.position.z);
                    break;
                }
                if (touch.phase == TouchPhase.Ended && m_currentDownTouchID == touch.fingerId)
                {
                    m_currentDownTouchID = -1;
                    // Maybe hide trampoline
                    break;
                }
            }
        }
        if(!m_isAnyFingerTouching)
            m_currentDownTouchID = -1;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = mycamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mycamera.nearClipPlane));
            balls[0].transform.position = new Vector3(worldPos.x, worldPos.y, balls[0].transform.position.z);
            OnFirstFingerDown();
        }
        // Mouse controls
        if ( Input.GetMouseButton(0) )
        {
            Vector2 worldPos = mycamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mycamera.nearClipPlane));
            balls[1].transform.position = new Vector3(worldPos.x, worldPos.y, balls[1].transform.position.z);
            m_isAnyFingerTouching = true;
        }



        Vector3 firstFinger = balls[0].transform.position;
        Vector3 secondFinger = balls[1].transform.position;

        switch (m_controlScheme)
        {
            case 1:
                trampoline.StretchToStartAndEnd(firstFinger, secondFinger);
                break;
            case 2:
                trampoline.StretchToStartAndEnd(firstFinger, secondFinger);
                break;
            case 3:
                trampoline.StretchToStartAndEnd(firstFinger - ( secondFinger - firstFinger ), secondFinger);
                break;
            default:
                break;
        }

    }


}
