﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFPS : MonoBehaviour
{

    void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
