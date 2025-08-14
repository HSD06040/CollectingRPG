using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatusController<T> : MonoBehaviour, IDamageable where T : BaseUnitData
{
    public T Data;

    public void Init(T data)
    {
        Data = data;
    }

    public void TakeDamage(int amount)
    {
        
    }
}
