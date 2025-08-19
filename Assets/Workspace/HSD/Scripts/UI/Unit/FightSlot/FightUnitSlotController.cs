using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightUnitSlotController : MonoBehaviour
{
    [SerializeField] GameObject _fightSlotPrefab;
    [SerializeField] Transform _content;
    [SerializeField] GridLayoutGroup _grid;
    [SerializeField] int _slotCount;
    private UI_FightUnitSlot[] _slots;

    private void Awake()
    {
        init();

        int count = _grid.transform.childCount;
        float width = ((RectTransform)_grid.transform).rect.width;
        float height = ((RectTransform)_grid.transform).rect.height;

        if (count > 0)
        {
            float cellWidth = width / count;
            _grid.cellSize = new Vector2(cellWidth, height);
        }
    }

    private void init()
    {
        _slots = new UI_FightUnitSlot[_slotCount];

        for (int i = 0; i < _slotCount; i++)
        {
            GameObject slot = Instantiate(_fightSlotPrefab, _content);
            _slots[i] = slot.GetComponent<UI_FightUnitSlot>();
            _slots[i].Init(null);
        }
    }

    public void SetUnit(UnitBase[] units)
    {
        int count = units.Length;

        for (int i = 0; i < units.Length; i++)
        {
            _slots[i].Init(units[i].StatusController);
            count--;
        }

        for (int i = _slots.Length - units.Length; i < _slots.Length; i++)
        {
            _slots[i].Init(null);
        }
    }
}
