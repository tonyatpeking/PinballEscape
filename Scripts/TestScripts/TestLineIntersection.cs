using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLineIntersection : MonoBehaviour
{

    public GameObject m_lineAStart;
    public GameObject m_lineAEnd;
    public GameObject m_lineBStart;
    public GameObject m_lineBEnd;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (MathUtils.DoLinesIntersect(
            m_lineAStart.transform.position,
            m_lineAEnd.transform.position,
            m_lineBStart.transform.position,
            m_lineBEnd.transform.position))
            Debug.Log("Intersect");
    }
}
