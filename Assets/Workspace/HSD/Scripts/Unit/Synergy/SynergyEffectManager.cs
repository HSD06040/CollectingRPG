using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SynergyEffectManager : MonoBehaviour
{
    #region Singleton
    private static SynergyEffectManager instance;
    public static SynergyEffectManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SynergyEffectManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        Init();
    }
    #endregion
    [SerializeField] Transform _center;
    
    public GlobalPassiveController GlobalPassiveController { get; private set; }

    private void Init()
    {
        GlobalPassiveController = new GlobalPassiveController(_center);
    }    
}
