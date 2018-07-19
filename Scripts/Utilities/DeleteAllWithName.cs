using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAllWithName : MonoBehaviour
{
    public List<string> m_namesToDelete;
    // Use this for initialization
    void Start()
    {
        foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            foreach( string nameToDelete in m_namesToDelete)
            {
                if( go.name == nameToDelete)
                {
                    Destroy(go);
                }
            }
        }
    }


}
