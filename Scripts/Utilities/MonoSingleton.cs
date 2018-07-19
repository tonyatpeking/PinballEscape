using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// We inherit from MonoBehaviour - meaning any
// MonoSingleton will potentially be a mono behaviour;
public abstract class MonoSingleton<T> : MonoBehaviour
   where T : MonoSingleton<T>
{
    static T s_instance;
    static bool s_quitting = false;
    protected bool m_markedForDelete = false;

    // We do this in Awake so that s_instance will be
    // set by the time GetInstance returns.  If we did it in start,
    // s_instance isn't set reliably until the next frame (Start calls)
    protected virtual void Awake()
    {
        if (s_instance == null)
        {
            // Detach from the parent, or else this will get deleted if parent is
            transform.parent = null;
            // First instance of this object created - set the main instance to it;
            s_instance = this as T;

            // Tell Unity to never destroy this object while it exist;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Singleton means single - just one.  If we
            // already have an instance we want to disallow a second, so we'll
            // immediately destroy this object.
            m_markedForDelete = true;
            GameObject.Destroy(gameObject);
        }
    }

    // If this is an instance, we can check if we're equal to the static global
    protected bool IsInstance()
    {
        return this == s_instance;
    }

    // Gets the instance.  Static meaning I can call this method anywhere without having
    // an object.  This will either return the existing instance, or if none exists,
    // creates a new one.
    public static T GetInstance()
    {
        if (s_instance == null)
        {
            // if we are quitting - do not create a new object
            // This is to prevent us creating objects if we access the
            // singleton during shutdown.
            if (s_quitting)
            {
                return null;
            }

            // Create a new game object in the scene.
            GameObject go = new GameObject();

            // Set the name to the name of this class preceded by a "_".
            // I use this to denote objects created from script - purely
            // personal style, not a standard convention
            go.name = "_" + typeof(T).Name;

            // Add this MonoBehaviour to the object.
            go.AddComponent<T>();
        }

        // Return our saved instanc.e
        return s_instance;
    }

    // Used to prevent accidently creating an instance during shutdown;
    private void OnApplicationQuit()
    {
        s_quitting = true;
    }
}
