using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DamageMeterController : MonoBehaviour
{
    [SerializeField] GameObject _damageMeterPrefab;
    [SerializeField] Transform _content;
    private DamageMeterSlot[] _damageMeterSlots;
    private List<DamageMeterSlot> _activeSlots = new List<DamageMeterSlot>();
    private CancellationTokenSource _cts;
    private int _unitCount;

    private void Awake()
    {
        CreateDamageMeterSlots();        
        DamageMeterSlot.OnDamaged += SortingDamageMeter;
    }
    private void OnDestroy()
    {
        DamageMeterSlot.OnDamaged -= SortingDamageMeter;
    }

    public void Init(UnitBase[] units)
    {
        _unitCount = units.Length;

        for (int i = 0; i < _damageMeterSlots.Length; i++)
        {
            if (i < _unitCount)
            {
                UnitStatusController status = units[i].StatusController;
                _damageMeterSlots[i].Init(status);
                _damageMeterSlots[i].gameObject.SetActive(false);
            }
            else
            {
                _damageMeterSlots[i].gameObject.SetActive(false);
            }
        }

        SortingDamageMeter();
    }

    private void CreateDamageMeterSlots()
    {
        int slotCount = UnitController.UnitMaxCount;
        _damageMeterSlots = new DamageMeterSlot[slotCount];

        for (int i = 0; i < slotCount; i++)
        {
            DamageMeterSlot slot = Instantiate(_damageMeterPrefab, _content).GetComponent<DamageMeterSlot>();
            _damageMeterSlots[i] = slot;
        }
    }

    private void SortingDamageMeter()
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        _activeSlots.Clear();

        foreach (var slot in _damageMeterSlots)
        {
            _activeSlots.Add(slot);
        }

        _activeSlots.Sort((a, b) => b.TotalDamage.CompareTo(a.TotalDamage));

        foreach (var slot in _damageMeterSlots)
            slot.gameObject.SetActive(false);

        // 상위 5개만 활성화
        int count = Mathf.Min(5, _unitCount);

        for (int i = 0; i < count; i++)
        {
            var slot = _activeSlots[i];
            slot.gameObject.SetActive(true);

            slot.transform.SetSiblingIndex(i);

            slot.SetNumber(i + 1);
        }

        SettingSliderMaxValue();
    }


    private async UniTaskVoid AnimateReorder(DamageMeterSlot slot, int targetIndex, float duration = 0.3f)
    {
        RectTransform rt = slot.GetComponent<RectTransform>();
        Vector2 startPos = rt.anchoredPosition;
        Vector2 endPos = ((RectTransform)_content.GetChild(targetIndex)).anchoredPosition;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            rt.anchoredPosition = Vector2.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, t));
            await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
        }

        slot.transform.SetSiblingIndex(targetIndex);
        rt.anchoredPosition = endPos;
    }

    private void SettingSliderMaxValue()
    {
        int maxValue = _activeSlots[0].TotalDamage;

        foreach (var slot in _damageMeterSlots)
        {
            slot.SetSliderMaxValue(maxValue);
            slot.RefreshValue();
        }
    }
}
