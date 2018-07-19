using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDrawDetector : MonoBehaviour
{
    public GameObject[] m_noDrawZones;
    public int BallInNoDrawLayer = 8;
    public int BallDefaultLayer = 9;
    // Use this for initialization
    void Start()
    {
        m_noDrawZones = GameObject.FindGameObjectsWithTag("NoDrawZone");
    }

    public bool IsInNoDrawZone()
    {
        foreach (GameObject noDrawZone in m_noDrawZones)
        {
            //if (noDrawZone.GetComponent<SpriteRenderer>().bounds.Contains(transform.position))
            //{
            //    return true;
            //}
            Bounds bounds = noDrawZone.GetComponent<Renderer>().bounds;
            bounds.min = new Vector3(bounds.min.x, bounds.min.y, -1);
            bounds.max = new Vector3(bounds.max.x, bounds.max.y, 1);
            if (bounds.Contains(transform.position))
            {
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInNoDrawZone())
            gameObject.layer = BallInNoDrawLayer;

        else
            gameObject.layer = BallDefaultLayer;
    }
}
