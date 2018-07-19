using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPointInTriangle : MonoBehaviour
{

    public GameObject m_p;
    public GameObject m_a;
    public GameObject m_b;
    public GameObject m_c;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (MathUtils.IsPointInTriangle(
            m_p.transform.position,
            m_a.transform.position,
            m_b.transform.position,
            m_c.transform.position))
            Debug.Log("Point is in triangle");
    }
}
