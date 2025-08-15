using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public Transform Target;
    public UnitData Data;
    public Animator Anim;

    [SerializeField] BaseFSM _fsm;
    [SerializeField] protected UnitStatusController _unitStatusController;

    protected virtual void Awake()
    {
        _unitStatusController.Init(Data);
    }

    protected virtual void Start()
    {
        _fsm.Init(this);
    }

    public bool SkillCheck()
    {
        if (_unitStatusController.CurMana.Value >= Data.UnitSkill.NeedMana)
        {
            _unitStatusController.CurMana.Value -= Data.UnitSkill.NeedMana;
            return true;
        }
        else 
            return false;
    }
}
