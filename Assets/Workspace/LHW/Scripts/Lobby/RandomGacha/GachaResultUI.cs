using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GachaResultUI : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private GameObject _gachaResultSlotUI;
    [SerializeField] private Transform _content;
    [SerializeField] private Image[] _gachaResultImages;

    private GameObject[] _slots = new GameObject[10];
    
    public void HeroGachaUpdate(UnitData data, int index)
    {
        if (_slots[index] == null)
        {
            _slots[index] = Instantiate(_gachaResultSlotUI, _content);
        }
        GachaResultUISlot slot = _slots[index].GetComponent<GachaResultUISlot>();
        slot.UpdateUI(data.Icon);
    }

    // 비활성화와 동시에 슬롯을 한개만 남겨두고 전부 파괴(Grid UI를 위해서 임시 처리)
    // 오브젝트 풀 반영할 수 있을 것 같습니다 -> 추후 풀 반영
    private void OnDisable()
    {
        if (_slots.Length <= 1) return;
        else
        {
            for (int i = 1; i < _slots.Length; i++)
            {
                Destroy(_slots[i]);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.SetActive(false);
    }
}