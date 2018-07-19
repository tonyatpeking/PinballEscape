using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{

    public GameObject ballToFollow;
    public GameObject topScroll;
    public GameObject bottomScroll;
    public GameObject leftScroll;
    public GameObject rightScroll;

    [Range(0.01f, 0.35f)]
    public float camTop = 0.2f;
    [Range(0.01f, 0.35f)]
    public float camSides = 0.2f;
    [Range(0.01f, 0.35f)]
    public float camBottom = 0.2f;

    enum CameraMode { TopOnly, AllDir, StopOnTouch };
    CameraMode cameraMode = CameraMode.AllDir;

    public void SetCamTop(float value)
    {
        camTop = value;
    }
    public void SetCamSides(float value)
    {
        camSides = value;
    }
    public void SetCamBottom(float value)
    {
        camBottom = value;
    }

    public void SetOnlyUp()
    {
        cameraMode = CameraMode.TopOnly;
    }

    public void SetAllDir()
    {
        cameraMode = CameraMode.AllDir;
    }

    public void SetStopOnTouch()
    {
        cameraMode = CameraMode.StopOnTouch;
    }

    // Update is called once per frame
    void Update()
    {

        PlayerPrefs.SetInt("camMode", (int)cameraMode);
        MoveScrollLines();

        // Move Camera
        Vector3 ballWorld = ballToFollow.transform.position;
        Vector3 ballScreen = WorldToScreen(ballWorld);

        if (cameraMode == CameraMode.TopOnly)
        {
            if (ballScreen.y > Screen.height * (1 - camTop))
            {
                float WorldOffsetY = ballWorld.y - CamTopWorldY();
                transform.Translate(0, WorldOffsetY, 0);
            }
        }
        if (cameraMode == CameraMode.AllDir)
        {
            if (ballScreen.y > Screen.height * (1 - camTop))
            {
                float WorldOffsetY = ballWorld.y - CamTopWorldY();
                transform.Translate(0, WorldOffsetY, 0);
            }
            if (ballScreen.y < Screen.height * camBottom)
            {
                float WorldOffsetY = ballWorld.y - CamBottomWorldY();
                transform.Translate(0, WorldOffsetY, 0);
            }
            if (ballScreen.x < Screen.width * camSides)
            {
                float WorldOffsetX = ballWorld.x - CamLeftWorldX();
                transform.Translate(WorldOffsetX, 0, 0);
            }
            if (ballScreen.x > Screen.width * (1 - camSides))
            {
                float WorldOffsetX = ballWorld.x - CamRightWorldX();
                transform.Translate(WorldOffsetX, 0, 0);
            }
        }

        if (cameraMode == CameraMode.StopOnTouch)
        {
            //test for touching
            bool isTouching = false;
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                {
                    isTouching = true;
                }
            }
            if (Input.GetMouseButton(0))
            {
                isTouching = true;
            }
            // if not touching, ease to ball position
            if (!isTouching)
            {
                Vector3 pos = ballToFollow.transform.position;
                pos.z = transform.position.z;
                Vector3 translateVector = (pos - transform.position) * 0.2f;
                if (translateVector.sqrMagnitude > 0.1f)
                    transform.Translate(translateVector);
                else
                    transform.position = pos;
            }
        }

    }

    void MoveScrollLines()
    {
        Vector3 newTopScroll = topScroll.transform.position;
        newTopScroll.y = CamTopWorldY();
        topScroll.transform.position = newTopScroll;

        Vector3 newBottomScroll = bottomScroll.transform.position;
        newBottomScroll.y = CamBottomWorldY();
        bottomScroll.transform.position = newBottomScroll;

        Vector3 newLeftScroll = leftScroll.transform.position;
        newLeftScroll.x = CamLeftWorldX();
        leftScroll.transform.position = newLeftScroll;

        Vector3 newRightScroll = rightScroll.transform.position;
        newRightScroll.x = CamRightWorldX();
        rightScroll.transform.position = newRightScroll;
    }

    float CamTopWorldY()
    {
        return ScreenToWorld(new Vector3(0, Screen.height * (1 - camTop), 0)).y;
    }

    float CamBottomWorldY()
    {
        return ScreenToWorld(new Vector3(0, Screen.height * camBottom, 0)).y;
    }

    float CamLeftWorldX()
    {
        return ScreenToWorld(new Vector3(Screen.width * camSides, 0, 0)).x;
    }

    float CamRightWorldX()
    {
        return ScreenToWorld(new Vector3(Screen.width * (1 - camSides), 0, 0)).x;
    }

    Vector3 WorldToScreen(Vector3 world)
    {
        return Camera.main.WorldToScreenPoint(world);
    }

    Vector3 ScreenToWorld(Vector3 screen)
    {
        return Camera.main.ScreenToWorldPoint(screen);
    }




}
