using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    static private T instance;
    static public T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();
                if (instance == null)
                {
                    instance = new GameObject(typeof(T) + "SingletonManager").AddComponent<T>();
                    instance.InitSelf();
                }
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this as T;
        InitSelf();
        DontDestroyOnLoad(instance.gameObject);
    }
    protected virtual void InitSelf()
    {
        //自动创建单例类的初始化方法
    }
}
