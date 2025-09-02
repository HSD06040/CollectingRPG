using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UnitPassiveController
{
    private UnitStatusController _owner;
    private Dictionary<string, UnitPassive> _passives = new Dictionary<string, UnitPassive>(128);

    public UnitPassiveController(UnitStatusController owner)
    {
        _owner = owner;
    }

    public void AddPassiveEffect(SynergyEffect effect, int multiplier = 1, bool isChange = false)
    {
        if (isChange)
        {
            RemovePassiveEffect(effect);
        }

        if (!_passives.ContainsKey(effect.Key))
        {
            _passives.Add(effect.Key, new UnitPassive(effect, _owner, multiplier));
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

    public void DeActiveAllPassive()
    {
        foreach (var passive in _passives.Values)
        {
            passive.Deactive();
        }
        _passives.Clear();
    }
}
