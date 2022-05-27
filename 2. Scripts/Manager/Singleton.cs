using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }


    protected void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<T>();
        }
    }

    private void OnDisable()
    {
        Instance = default(T);
    }
}
