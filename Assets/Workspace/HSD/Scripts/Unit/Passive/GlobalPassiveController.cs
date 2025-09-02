using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPassiveController
{
    private Transform _center;
    private Dictionary<string, GlobalPassive> _passives = new Dictionary<string, GlobalPassive>(128);

    public GlobalPassiveController(Transform center)
    {
        _center = center;
    }

    public void AddPassiveEffect(SynergyEffect effect, UnitBase[] units, int multiplier = 1, bool isChange = false)
    {
        if (isChange)
        {
            RemovePassiveEffect(effect);
        }

        if (!_passives.ContainsKey(effect.Key))
        {
            _passives.Add(effect.Key, new GlobalPassive(effect, units, _center, multiplier));          
            _passives[effect.Key].Active();
        }
    }

    public void RemovePassiveEffect(SynergyEffect effect)
    {
        if (_passives.ContainsKey(effect.Key))
        {
            _passives[effect.Key].Deactive();
            _passives.Remove(effect.Key);
        }
    }
}
