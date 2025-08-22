using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PresetSelectUnit : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Sprite _xImage;

    [Header("Index")]
    [SerializeField] private int _index;

    [Header("Buttons")]
    [SerializeField] private GameObject _activePartyButton;
    [SerializeField] private GameObject _disabledPartyButton;
    [SerializeField] private GameObject _lockedPartyButton;
    [SerializeField] private GameObject _highlightPanel;

    [Header("UI")]
    [SerializeField] private Image[] _presetImages;
    [SerializeField] private TMP_Text _leaderName;
    [SerializeField] private TMP_Text _partyDamageText;
    [SerializeField] private TMP_Text _synergy1Text;
    [SerializeField] private TMP_Text _synergy2Text;


    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        // 프리셋이 활성화가 안 되어 있으면 잠겨 있다고 표시하는 UI 출력
        if(TempDataManager.Instance.PresetData.Count < _index + 1)
        {
            SetactiveGameobject("LockedPartyButton");
            Debug.Log("잠김");
            return;
        }

        // 프리셋이 세팅되어 있지 않으면(리더가 없는 상태이면) 프리셋 추가 UI 출력
        if (TempDataManager.Instance.PresetData[_index].Statuses[0].Data == null)
        {
            SetactiveGameobject("DisabledPartyButton");
            Debug.Log("비활성화됨");
            return;
        }

        // 프리셋이 사용 가능한 상태이면 하단의 과정 진행
        SetactiveGameobject("ActivePartyButton");
        UpdateUI();
    }

    private void UpdateUI()
    {
        UnitStatus[] preset = TempDataManager.Instance.PresetData[_index].Statuses;
        int damage = 0;

        for (int i = 0; i < preset.Length; i++)
        {
            if (preset[i].Data != null)
            {
                _presetImages[i].color = Color.white;
                _presetImages[i].sprite = preset[i].Data.Icon;
                damage += preset[i].CombatPower;
            }
            else
            {
                _presetImages[i].color = Color.black;
                _presetImages[i].sprite = _xImage;
            }
        }
        _leaderName.text = preset[0].Data.Name;
        _partyDamageText.text = $"Party Damage : {damage}";
        // 시너지 입력 방식은 시너지 활성화 기능 구현 이후 진행
        //_synergy1Text.text = preset[0].Data.EnhancementData.ClassSynergy.ToString();
        //_synergy2Text.text = preset[0].Data.EnhancementData.ClassSynergy.ToString();
        Debug.Log("활성화됨");
    }

    private void SetactiveGameobject(string activeObject)
    {
        _activePartyButton.SetActive(activeObject.Equals(_activePartyButton.name));
        _disabledPartyButton.SetActive(activeObject.Equals(_disabledPartyButton.name));
        _lockedPartyButton.SetActive(activeObject.Equals(_lockedPartyButton.name));
    }

    public void ActiveHighlight(bool highlighted)
    {
        _highlightPanel.SetActive(highlighted);
    }
}
