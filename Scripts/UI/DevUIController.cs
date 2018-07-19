using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DevUIController : MonoBehaviour
{
    public Slider m_camZoom;
    public Slider m_camTop;
    public Slider m_camSides;
    public Slider m_camBot;
    public Slider m_ballBounce;
    public Slider m_gravity;
    public Slider m_ballSpeed;
    //public Slider m_gobalTimeScale;
    public Slider m_touchTimeScale;
    public Button m_nextScene;

    public float m_camZoomPreset = 11.1f;
    public float m_camTopPreset = 0.1f;
    public float m_camSidesPreset = 0.1f;
    public float m_camBotPreset = 0.1f;
    public float m_ballBouncePreset = 0.5f;
    public float m_gravityPreset = 0.7f;
    public float m_ballSpeedPreset = 9.5f;
    //public float m_touchTimeScalePreset = 0.1f;

    public Dictionary<Slider, float> m_presetValues = new Dictionary<Slider, float>();

    public string m_dataSet = "0";

    public float defaultGravity = -9.81f;
    public GameObject m_ball;

    public List<Slider> m_allSliders = new List<Slider>();
    void Awake()
    {


        m_presetValues = new Dictionary<Slider, float>()
        {
            { m_camZoom, m_camZoomPreset },
            { m_camTop, m_camTopPreset },
            { m_camSides, m_camSidesPreset },
            { m_camBot, m_camBotPreset },
            { m_ballBounce, m_ballBouncePreset },
            { m_gravity, m_gravityPreset },
            { m_ballSpeed, m_ballSpeedPreset },
            //{ m_touchTimeScale, m_touchTimeScalePreset },
        };

        foreach (KeyValuePair<Slider, float> entry in m_presetValues)
        {
            SetSliderValue(entry.Key, entry.Value);
        }


        m_allSliders.Clear();
        m_allSliders.Add(m_camZoom);
        m_allSliders.Add(m_camTop);
        m_allSliders.Add(m_camSides);
        m_allSliders.Add(m_camBot);
        m_allSliders.Add(m_ballBounce);
        m_allSliders.Add(m_gravity);
        m_allSliders.Add(m_ballSpeed);
        //m_allSliders.Add(m_gobalTimeScale);
        m_allSliders.Add(m_touchTimeScale);

        //foreach (Slider slider in m_allSliders)
        //{
        //    LoadSliderValue(slider);
        //}

        LoadSliderValue(m_touchTimeScale);
        m_touchTimeScale.onValueChanged.RemoveAllListeners();
        m_touchTimeScale.onValueChanged.AddListener(
            delegate
            {
                SaveSliderValues(m_touchTimeScale);
                SetValueDisplay(m_touchTimeScale);
            });


        foreach (Slider slider in m_allSliders)
        {
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(
                delegate
                {
                    //SaveSliderValues(slider);
                    SetValueDisplay(slider);
                });
        }

        LoadSliderValue(m_touchTimeScale);
        m_touchTimeScale.onValueChanged.RemoveAllListeners();
        m_touchTimeScale.onValueChanged.AddListener(
            delegate
            {
                SaveSliderValues(m_touchTimeScale);
                SetValueDisplay(m_touchTimeScale);
            });

        AddListenerAndExecute(m_camZoom, OnCamZoom);
        AddListenerAndExecute(m_camTop, OnCamTop);
        AddListenerAndExecute(m_camSides, OnCamSides);
        AddListenerAndExecute(m_camBot, OnCamBot);

        AddListenerAndExecute(m_ballBounce, OnBallBounce);
        AddListenerAndExecute(m_gravity, OnGravity);
        AddListenerAndExecute(m_ballSpeed, OnBallSpeed);
        //AddListenerAndExecute(m_gobalTimeScale, OnGlobalTimeScale);
        AddListenerAndExecute(m_touchTimeScale, OnTouchTimeScale);

        m_nextScene.onClick.AddListener(OnNextScene);
    }

    void AddListenerAndExecute(Slider slider, UnityEngine.Events.UnityAction<float> action)
    {
        slider.onValueChanged.AddListener(action);
        action(slider.value);
    }

    void OnCamZoom(float value)
    {
        //Camera.main.orthographicSize = value;
    }

    void OnCamTop(float value)
    {
        //Camera.main.GetComponent<CameraController>().SetCamTop(value);
    }

    void OnCamSides(float value)
    {
        //Camera.main.GetComponent<CameraController>().SetCamSides(value);
    }

    void OnCamBot(float value)
    {
        //Camera.main.GetComponent<CameraController>().SetCamBottom(value);
    }

    void OnBallBounce(float value)
    {
        m_ball.GetComponent<BouncinessController>().SetBounciness(value);
    }

    void OnGravity(float value)
    {
        Physics2D.gravity = new Vector2(0, value * defaultGravity);
    }

    void OnBallSpeed(float value)
    {
        m_ball.GetComponent<ClampSpeed>().SetMaxSpeed(value);
    }

    //void OnGlobalTimeScale(float value)
    //{
    //    GetComponent<SlowTimeOnTouch>().SetGlobalTimeScale(value);
    //}

    void OnTouchTimeScale(float value)
    {
        //GetComponent<SlowTimeOnTouch>().SetTouchTimeScale(value);
    }

    void OnNextScene()
    {
        SceneLoader.GetInstance().LoadNextSceneWithFade();
    }

    void LoadSliderValue(Slider slider)
    {
        string name = slider.transform.parent.name + m_dataSet;
        slider.value = PlayerPrefs.GetFloat(name, slider.value);

        // Set text
        SetValueDisplay(slider);
    }

    void SetSliderValue(Slider slider, float value)
    {
        slider.value = value;
        SetValueDisplay(slider);
    }

    void SaveSliderValues(Slider slider)
    {
        string name = slider.transform.parent.name + m_dataSet;
        PlayerPrefs.SetFloat(name, slider.value);
    }



    void Update()
    {

    }

    void SetValueDisplay(Slider slider)
    {
        slider.transform.parent.Find("Panel").Find("ValueDisplay").GetComponent<Text>().text = slider.value.ToString("0.00");
    }

    public void SetTextWithFloat(float value)
    {
        Text text = GetComponent<Text>();
        if (text != null)
        {
            text.text = value.ToString("0.00");
        }
    }
}
