using Map;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EventDataDB", menuName = "Data/EventDataDB")]
public class EventDataDB : ScriptableObject
{
    [Header("지역별 고정 이벤트")] public EventData[] _regionEvents = new EventData[7];
    [Header("공용 이벤트")] public EventData[] _commonEvents;

    public EventData GetRandomEvent(int _regionNumber)
    {
        List<EventData> _availableEvents = new List<EventData>();

        int _regionIndex = _regionNumber - 1;

        if (_regionIndex >= 0 && _regionIndex < _regionEvents.Length)
        {
            if (_regionEvents[_regionIndex] != null)
            {
                _availableEvents.Add(_regionEvents[_regionIndex]);
            }
        }

        if (_commonEvents != null && _commonEvents.Length > 0)
        {
            _availableEvents.AddRange(_commonEvents.Where(e=>e!=null));
        }

        if (_availableEvents.Count > 0)
        {
            int _randomIndex = Random.Range(0, _availableEvents.Count);
            return _availableEvents[_randomIndex];
        }
        return null;
    }

    public EventData GetRandomCommonEvent()
    {
        if (_commonEvents == null || _commonEvents.Length == 0)
            return null;

        var _validEvents = _commonEvents.Where(e => e != null).ToArray();
        if (_validEvents.Length == 0)
            return null;
        
        return _validEvents[Random.Range(0, _validEvents.Length)];
    }

    public EventData GetRegionFixedEvent(int _regionNumber)
    {
        int _regionIndex = _regionNumber - 1;
        if (_regionIndex >= 0 && _regionIndex < _regionEvents.Length)
        {
            return _regionEvents[_regionIndex];
        }

        return null;
    }
}
