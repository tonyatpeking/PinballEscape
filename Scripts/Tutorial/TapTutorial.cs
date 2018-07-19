using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapTutorial : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool tapUp = false;
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Ended )
            {
                tapUp = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            tapUp = true;
        }

        if( tapUp )
        {
            gameObject.SetActive(false);
        }
    }
}
