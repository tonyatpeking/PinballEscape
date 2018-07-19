using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestShowBounds : MonoBehaviour
{
    public Bounds bounds;
    public Vector3 min;
    public Vector3 max;

    public GameObject lineStart;
    public GameObject lineEnd;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bounds = GetComponent<SpriteRenderer>().bounds;
        min = bounds.min;
        max = bounds.max;
        CheckLineIntersect();
    }

    void CheckLineIntersect()
    {
        float distance;
        Ray rayA = new Ray(lineStart.transform.position, lineEnd.transform.position - lineStart.transform.position);
        bool intersectA = bounds.IntersectRay(rayA, out distance);
        Ray rayB = new Ray(lineEnd.transform.position, lineStart.transform.position - lineEnd.transform.position);
        bool intersectB = bounds.IntersectRay(rayB);
        //if (intersectA && intersectB)
        //    GetComponent<SpriteRenderer>().color = Color.red;
        //else
        //    GetComponent<SpriteRenderer>().color = Color.white;

    }
}
