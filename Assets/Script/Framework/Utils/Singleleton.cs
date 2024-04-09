using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleleton<T> where T : new()
{
    private static T _instance;
    private static object mutex = new object();
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (mutex) // ��֤�̰߳�ȫ
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}


public class UnitySingleton<T> : MonoBehaviour where T : Component
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType(typeof(T)) as T;
                if (instance != null)
                {
                    GameObject obj = new GameObject();
                    instance = (T)obj.AddComponent(typeof(T));
                    // obj.hideFlags = HideFlags.HideAndDontSave;
                    obj.name = typeof(T).Name;
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }
}