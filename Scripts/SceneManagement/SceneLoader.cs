using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoSingleton<SceneLoader>
{
    public string m_mainMenu = "MainMenu";
    int m_mainMenuBuildIdx = -1;



    override protected void Awake()
    {
        base.Awake();
    }

    public void Start()
    {
        m_mainMenuBuildIdx = SceneManager.GetSceneByName(m_mainMenu).buildIndex;
    }

    public bool IsMainMenu()
    {
        return (SceneManager.GetActiveScene().buildIndex == 0);
    }


    public void ResetScene()
    {
        LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(m_mainMenu);
    }

    public void LoadScene(int sceneBuildIdx)
    {
        SceneManager.LoadScene(sceneBuildIdx);
    }

    public int GetNextLevelIdx()
    {
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int nextSceneIdx = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIdx >= totalScenes)
            nextSceneIdx = 0;
        if (nextSceneIdx == m_mainMenuBuildIdx)
        {
            ++nextSceneIdx;
            if (nextSceneIdx >= totalScenes)
                nextSceneIdx = 0;
        }
        return nextSceneIdx;
    }

    public void LoadNextScene()
    {
        LoadScene(GetNextLevelIdx());
    }

    public void LoadNextSceneWithFade()
    {
        LoadSceneWithFade(GetNextLevelIdx());
    }

    public void LoadSceneWithFade(int sceneBuildIdx)
    {
        ScreenFader.GetInstance().FadeToBlack(0.5f,
            delegate
            {
                LoadScene(sceneBuildIdx);
                ScreenFader.GetInstance().FadeToTransparent(1f);
            });
    }

    public void LoadSceneWithFade(string sceneBuildIdx)
    {
        ScreenFader.GetInstance().FadeToBlack(0.5f,
            delegate
            {
                SceneManager.LoadScene(sceneBuildIdx);
                ScreenFader.GetInstance().FadeToTransparent(1f);
            });
    }

    public void LoadMainMenuLevelSelect()
    {
        SceneManager.LoadScene(m_mainMenu);
        StartCoroutine(DelayedLevelSelect());
    }

    IEnumerator DelayedLevelSelect()
    {
        string sceneName = "";

        while (sceneName != m_mainMenu)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            sceneName = currentScene.name;
            yield return null;
        }
        //yield return new WaitForSeconds(0.1f);
        LevelSelectController levelSelect = GameObject.FindObjectOfType<LevelSelectController>();
        Animator anim = levelSelect.transform.parent.GetComponent<Animator>();
        anim.SetBool("slideIn", true);
    }
}
