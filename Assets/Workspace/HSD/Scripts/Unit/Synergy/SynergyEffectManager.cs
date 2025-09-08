using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SynergyEffectManager : InGameSingleton<SynergyEffectManager>
{
    [SerializeField] Transform _center;

    protected override void Awake()
    {
        Init();
    }   
    
    public GlobalPassiveController GlobalPassiveController { get; private set; }

    private void Init()
    {
        GlobalPassiveController = new GlobalPassiveController(_center);
    }    
}
