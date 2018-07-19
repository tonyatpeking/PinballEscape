using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreController : MonoSingleton<ScoreController>
{

    public int m_slingshotScore;

    public int m_blueScore;

    public int m_purpleFirstScore;
    public int m_purpleScore;
    public int m_purpleDoorScore;

    public int m_orangeFirstScore;
    public int m_orangeScore;
    public int m_orangeDoorScore;

    public int m_winScore;

    public int m_currentScore = 0;
    public int m_levelScore = 0;

    public float m_timePlayed;

    float m_scorePreWarm = 0.2f;


    public Text m_scoreText;
    public GameObject m_scorePanel;
    public Text m_highScore;
    public GameObject m_highscorePanel;

    GameObject m_scoreLocation;

    // Use this for initialization
    override protected void Awake()
    {
        base.Awake();
        if (m_markedForDelete)
            return;
        m_currentScore = 0;
        m_scoreText.text = m_currentScore.ToString();
        m_scoreLocation = GameObject.Find("ScoreLocation");
        SceneManager.activeSceneChanged +=
            delegate
            {
                ResetLevelScore();
                CheckIsMainMenu();
            };
    }

    void CheckIsMainMenu()
    {
        if (SceneLoader.GetInstance().IsMainMenu())
        {
            m_scorePanel.SetActive(false);
            m_highscorePanel.SetActive(true);
            int highScore = PlayerPrefs.GetInt("Highscore");
            m_highScore.text = "Current Highscore:\n" + highScore.ToString();
            SetScore(0);
            m_levelScore = 0;
        }
        else
        {
            m_scorePanel.SetActive(true);
            m_highscorePanel.SetActive(false);
        }
    }

    void ResetLevelScore()
    {
        m_levelScore = 0;
    }

    public GameObject GetScoreLocation()
    {
        if (!m_scoreLocation)
            m_scoreLocation = GameObject.Find("ScoreLocation");
        return m_scoreLocation;
    }

    public void AddScore(int scoreToAdd)
    {
        m_currentScore += scoreToAdd;
        m_levelScore += scoreToAdd;
        SetScoreText();
    }

    public void AddScoreWithDelay(int scoreToAdd)
    {
        float delay = ParticleSystemSpawner.GetInstance().GetPrototype().GetParticleLifeTime() - m_scorePreWarm;
        StartCoroutine(ScoreWithDelay(delay, scoreToAdd));
    }

    IEnumerator ScoreWithDelay( float delay, int scoreToAdd )
    {
        yield return new WaitForSeconds(delay);
        AddScore(scoreToAdd);
    }

    public void SetScore(int score)
    {
        m_currentScore = score;
        SetScoreText();
    }
    public int GetScore()
    {
        return m_currentScore;
    }

    public int GetLevelScore()
    {
        return m_levelScore;
    }

    void SetScoreText()
    {
        m_scoreText.text = m_currentScore.ToString();

    }

    public void AddBumperSlingshotScore(ColorSet color)
    {
        switch (color)
        {
            case ColorSet.BLUE:
                AddScoreWithDelay(m_blueScore);
                break;
            case ColorSet.PURPLE:
                AddScoreWithDelay(m_purpleScore);
                break;
            case ColorSet.ORANGE:
                AddScoreWithDelay(m_orangeScore);
                break;
            case ColorSet.SLINGSHOT:
                AddScoreWithDelay(m_slingshotScore);
                break;
            default:
                break;
        }
    }

    public int GetBumperSlingshotScore(ColorSet color)
    {
        switch (color)
        {
            case ColorSet.BLUE:
                return m_blueScore;
            case ColorSet.PURPLE:
                return m_purpleScore;
            case ColorSet.ORANGE:
                return m_orangeScore;
            case ColorSet.SLINGSHOT:
                return m_slingshotScore;
            default:
                return 0;
        }
    }

    public void AddBumperFristScore(ColorSet color)
    {
        switch (color)
        {
            case ColorSet.PURPLE:
                AddScoreWithDelay(m_purpleFirstScore);
                break;
            case ColorSet.ORANGE:
                AddScoreWithDelay(m_orangeFirstScore);
                break;
            default:
                break;
        }
    }

    public int GetBumperFristScore(ColorSet color)
    {
        switch (color)
        {
            case ColorSet.PURPLE:
                return m_purpleFirstScore;
            case ColorSet.ORANGE:
                return m_orangeFirstScore;
            default:
                return 0;
        }
    }

    public void AddDoorScore(ColorSet color)
    {
        switch (color)
        {
            case ColorSet.PURPLE:
                AddScore(m_purpleDoorScore);
                break;
            case ColorSet.ORANGE:
                AddScore(m_orangeDoorScore);
                break;
            default:
                break;
        }
    }

    public void AddWinScore()
    {
        AddScore(m_winScore);
    }



}
