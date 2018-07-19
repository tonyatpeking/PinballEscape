using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAllCubes : MonoBehaviour {



    List<GameObject> goList = new List<GameObject>();
    string nameToAdd = "Cube";

    void Start()
    {
        foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == nameToAdd)
            {
                Destroy(go);
            }
        }
    }
}
