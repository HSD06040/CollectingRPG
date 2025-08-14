using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public BaseUnitData Data;
    public Animator Anim;

    [SerializeField] BaseFSM _fsm;

    protected virtual void Start()
    {
        _fsm.Init(this);
    }

    private void Update()
    {
        
    }
}
