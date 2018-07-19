using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MusicMonoSingleton : MonoSingleton<MusicMonoSingleton>
{
    public AudioSource m_endPortalAS;
    //public AudioSource LevelMusic;
    //public AudioClip HardLevelMusic;
    public AudioClip EasyLevelMusic;

    private void Update()
    {
        //PlayEndLevelSound();
    }

    protected override void Awake()
    {
        base.Awake();

        if (m_markedForDelete)
            return;

        SceneManager.activeSceneChanged += delegate
        {

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu")
            {
                gameObject.GetComponent<AudioSource>().enabled = false;
            }
            else {
                gameObject.GetComponent<AudioSource>().enabled = true;
            }
            
            /*if (!gameObject.GetComponent<AudioSource>().isPlaying) {
                gameObject.GetComponent<AudioSource>().Play();
            }*/


        };

    }

    public void PlayEndLevelSound()
    {
        if (m_endPortalAS)
            m_endPortalAS.Play();



    }
}
