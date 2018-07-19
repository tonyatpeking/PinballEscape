using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject m_pausePanel;
    public Button m_pause;
    public Button m_resume;
    public Button m_restart;
    public Button m_mainMenu;
    public Button m_levelSelect;
    public Button m_quit;
    public SlowTimeOnTouch m_slowTime;

    bool m_isPaused = false;

    public AudioSource ButtonSound;

    void Start()
    {
        m_pause.onClick.AddListener(OnPause);
        m_resume.onClick.AddListener(OnResume);
        m_restart.onClick.AddListener(OnRestart);
        m_mainMenu.onClick.AddListener(OnMainMenu);
        m_levelSelect.onClick.AddListener(OnLevelSelect);
        m_quit.onClick.AddListener(OnQuit);
    }


    private void OnPause()
    {
        if (m_isPaused)
        {
            OnResume();
        }
        else
        {
            ButtonSound.Play();

            m_isPaused = true;
            //m_slowTime.SetIsPaused(true);
            GameStateController.GetInstance().SetPaused(true);
            m_pausePanel.SetActive(true);
        }
    }

    private void OnResume()
    {
        ButtonSound.Play();

        m_isPaused = false;
        GameStateController.GetInstance().SetPaused(false);
        m_pausePanel.SetActive(false);
    }

    private void OnRestart()
    {
        ButtonSound.Play();

        GameStateController.GetInstance().SetPaused(false);
        SceneLoader.GetInstance().ResetScene();
    }

    private void OnMainMenu()
    {
        ButtonSound.Play();

        GameStateController.GetInstance().SetPaused(false);
        SceneLoader.GetInstance().LoadMainMenu();
    }

    private void OnLevelSelect()
    {
        ButtonSound.Play();

        GameStateController.GetInstance().SetPaused(false);
        SceneLoader.GetInstance().LoadMainMenuLevelSelect();
    }

    private void OnQuit()
    {
        ButtonSound.Play();

        Application.Quit();
    }

}
