using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DisableRendererOnAwake : MonoBehaviour
{

    void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }


}
