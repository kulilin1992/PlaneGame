using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenSingleton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }
    // private static T instance;
    // public static T Instance
    // {
    //     get
    //     {
    //         if (instance == null)
    //         {
    //             instance = FindObjectOfType<T>();
    //             if (instance == null)
    //             {
    //                 GameObject go = new GameObject(typeof(T).Name);
    //                 instance = go.AddComponent<T>();
    //             }
    //         }
    //         return instance;
    //     }
    // }
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            //DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
