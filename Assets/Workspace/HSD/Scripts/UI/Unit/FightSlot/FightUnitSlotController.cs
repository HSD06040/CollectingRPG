using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class FightUnitSlotController : MonoBehaviour
{
    [SerializeField] GameObject _fightSlotPrefab;
    [SerializeField] Transform _content;
    [SerializeField] GridLayoutGroup _grid;
    private UI_FightUnitSlot[] _slots;

    private void Awake()
    {
        CreateSlots();

        int count = _grid.transform.childCount;
        float width = ((RectTransform)_grid.transform).rect.width;
        float height = ((RectTransform)_grid.transform).rect.height;

        if (count > 0)
        {
            float cellWidth = width / count;
            _grid.cellSize = new Vector2(cellWidth, height);
        }
    }

    private void CreateSlots()
    {
        int _slotCount = UnitController.UnitMaxCount;

        _slots = new UI_FightUnitSlot[_slotCount];

        for (int i = 0; i < _slotCount; i++)
        {
            GameObject slot = Instantiate(_fightSlotPrefab, _content);
            _slots[i] = slot.GetComponent<UI_FightUnitSlot>();
            _slots[i].Init(null);
        }
    }

    public void Init(UnitBase[] units)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (units[i] != null)
            {
                _slots[i].Init(units[i].StatusController);
            }
            else
            {
                _slots[i].Init(null);
            }
        }
    }
}
