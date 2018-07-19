using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelSelectController : MonoBehaviour
{
    public Button[] m_buttons;

    void Awake()
    {
        m_buttons = GetComponentsInChildren<Button>();
        for( int i = 0; i < m_buttons.Length; ++i )
        {
            int sceneIdx = i + 1;
            m_buttons[i].onClick.AddListener(
                delegate
                {
                    SceneLoader.GetInstance().LoadSceneWithFade(sceneIdx);
                });
        }
    }



    void Update()
    {

    }
}
