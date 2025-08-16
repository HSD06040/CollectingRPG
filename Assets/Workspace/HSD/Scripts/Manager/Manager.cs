using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Manager
{
    public static ResourcesManager Resources => ResourcesManager.Instance;
    public static PoolManager Pool => PoolManager.Instance;
    public static GameManager Game => GameManager.Instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        ResourcesManager.CreateInstance();
        PoolManager.CreateInstance();
        GameManager.CreateInstance();
    }
}
