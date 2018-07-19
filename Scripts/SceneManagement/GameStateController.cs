using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateController : MonoSingleton<GameStateController>
{

    bool m_isPaused = false;

    float m_savedFixedDeltaTime = 0.02f;

    public GameObject m_levelCompletePanel;
    public GameObject m_victoryPanel;
    public GameObject m_pauseButton;
    ScoreController m_scoreController;

    public Text m_levelScore;
    public Text m_totalScore;
    public Text m_victoryScore;

    public Button m_nextLevel;
    public Button m_endGameNextLevel;

    public string m_highscoreString = "Highscore";

    protected override void Awake()
    {
        base.Awake();
        if (m_markedForDelete)
            return;
        m_savedFixedDeltaTime = Time.fixedDeltaTime;
        m_scoreController = GetComponent<ScoreController>();


    }

    private void Start()
    {

        m_nextLevel.onClick.AddListener(
        delegate
        {
            m_levelCompletePanel.SetActive(false);
            SceneLoader.GetInstance().LoadNextScene();
            SetPaused(false);
        });

        m_endGameNextLevel.onClick.AddListener(
        delegate
        {
            m_victoryPanel.SetActive(false);
            SceneLoader.GetInstance().LoadMainMenu();
            SetPaused(false);
        });
    }



    public void LevelComplete()
    {
        SetPaused(true);
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            Victory();
        }
        else
        {
            m_levelCompletePanel.SetActive(true);
            m_levelScore.text = "< Level Score: " + m_scoreController.GetLevelScore().ToString() + " >";
            m_totalScore.text = "< Total Score: " + m_scoreController.GetScore().ToString() + " >";
        }
    }

    public void Victory()
    {
        m_victoryPanel.SetActive(true);
        int score = m_scoreController.GetScore();
        string victoryScore = "< Total Score: " + score.ToString() + " >";
        if (score > PlayerPrefs.GetInt(m_highscoreString, 0))
        {
            victoryScore += "\nNew High Score!";
            PlayerPrefs.SetInt(m_highscoreString, score);
        }
        m_victoryScore.text = victoryScore;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsPaused()
    {
        return m_isPaused;
    }

    public void SetPaused(bool isPaused)
    {
        m_isPaused = isPaused;
        if (isPaused)
        {
            //Time.fixedDeltaTime = 0f;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            //Time.fixedDeltaTime = m_savedFixedDeltaTime;
        }
    }

}
