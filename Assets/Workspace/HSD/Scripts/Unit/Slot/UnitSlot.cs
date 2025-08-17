using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSlot : MonoBehaviour
{
    private SpriteRenderer _sr;
    private int _line;
    private Vector2 _pos;
    private UnitBase _unit;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();        
    }

    public void Init(int line, Vector2 pos)
    {
        _line = line;
        _pos = pos;
    }
    
    public void CheckUnit(UnitBase unit)
    {

    }

    public void SetUnit(UnitBase unit)
    {
        if (unit == null) return;
        
        unit.gameObject.transform.position = transform.position;
        unit.gameObject.transform.SetParent(transform);

        _unit = unit;
    }
}
