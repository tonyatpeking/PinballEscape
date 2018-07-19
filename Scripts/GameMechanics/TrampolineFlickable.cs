using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineFlickable : MonoBehaviour {

    public Vector3 m_startPos;
    public Vector3 m_endPos;

    public GameObject m_touch1;
    public GameObject m_touch2;

    HingeJoint2D m_hinge;
    TargetJoint2D m_target;

    public void Awake()
    {
        m_hinge = GetComponent<HingeJoint2D>();
        m_target = GetComponent<TargetJoint2D>();
    }

    public void SetStartAndEnd( Vector3 start, Vector3 end )
    {
        m_startPos = start;
        m_endPos = end;
    }

	void Start () {
        m_startPos = transform.position;
        m_endPos = transform.position;
    }

	void Update () {

        SetStartAndEnd(m_touch1.transform.position, m_touch2.transform.position);



        //// Transform
        //transform.position = (m_startPos + m_endPos) / 2f;

        //// Rotation
        //Vector2 normalizedDirVector = (m_startPos - m_endPos).normalized;
        //float angle = Mathf.Atan2(normalizedDirVector.y, normalizedDirVector.x);
        //transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle);

        // Scale
        float width = (m_startPos - m_endPos).magnitude;
        transform.localScale = new Vector3(width, transform.localScale.y, transform.localScale.z);

        m_hinge.connectedAnchor = m_startPos;
        m_target.target = m_endPos;
    }

    public void StretchToStartAndEnd(Vector3 start, Vector3 end)
    {
        SetStartAndEnd(start, end);

        // Transform
        transform.position = (m_startPos + m_endPos) / 2f;

        // Rotation
        Vector2 normalizedDirVector = (m_startPos - m_endPos).normalized;
        float angle = Mathf.Atan2(normalizedDirVector.y, normalizedDirVector.x);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle);

        // Scale
        float width = (m_startPos - m_endPos).magnitude;
        transform.localScale = new Vector3(width, transform.localScale.y , transform.localScale.z);
    }

}
