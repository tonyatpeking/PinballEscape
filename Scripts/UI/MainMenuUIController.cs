using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    public Button m_start;
    public Button m_levelSelectBack;
    public Button m_credits;
    public GameObject m_creditsPanel;
    public Button m_exit;
    public Button m_creditsBack;
    public Animator m_levelSelectAnim;

    public AudioSource ButtonSound;

    void Start()
    {
        m_start.onClick.AddListener(OnStart);
        m_credits.onClick.AddListener(OnCredits);
        m_exit.onClick.AddListener(OnExit);
        m_creditsBack.onClick.AddListener(OnCreditsBack);
        m_levelSelectBack.onClick.AddListener(OnLevelSelectBack);
    }

    void OnStart()
    {
        ButtonSound.Play();
        //SceneLoader.GetInstance().LoadNextSceneWithFade();
        //SceneLoader.GetInstance().LoadNextScene();
        m_levelSelectAnim.SetBool("slideIn", true);
    }

    void OnLevelSelectBack()
    {
        ButtonSound.Play();
        m_levelSelectAnim.SetBool("slideIn", false);
    }

    void OnCredits()
    {
        ButtonSound.Play();
        m_creditsPanel.SetActive(true);
    }

    void OnExit()
    {
        Application.Quit();

    }

    void OnCreditsBack()
    {
        ButtonSound.Play();
        m_creditsPanel.SetActive(false);
    }
}
