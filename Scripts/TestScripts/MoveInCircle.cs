using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInCircle : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        float r = 20;
        float angle = Time.time * 3f;
        Vector3 newRot = transform.rotation.eulerAngles;
        newRot.x = Mathf.Cos(angle) * r;
        newRot.y = Mathf.Sin(angle) * r;
        transform.rotation = Quaternion.Euler(newRot);
    }
}
